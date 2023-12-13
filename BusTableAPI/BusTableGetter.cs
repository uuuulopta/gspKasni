namespace gspAPI.BusTableAPI;

using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Entities;
using gspAPI.Models;
using gspApiGetter.BusTableAPI;
using HtmlAgilityPack;
using Mappings;
using Services;
using StackExchange.Profiling;

public class BusTableGetter : IBusTableGetter
{
    private readonly HttpClient _client = new();
    private readonly IBusTableRepository _busTableRepository;
    public BusTableGetter(IBusTableRepository btr,HttpClient? client = null)
    {
        if (client == null)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/html"));
        }
        else _client = client;

        _busTableRepository = btr;
    }

    IEnumerable<BusTableDto> _getBusTablesFromHtml(string htmlString)
    {

        using (MiniProfiler.Current.Step("_getBusTablesFromHTML"))
        {


            var html = new HtmlDocument();
            html.LoadHtml(htmlString);
            var tables = html.DocumentNode.SelectNodes("//table");
            var busTables = new List<BusTableDto>();
            var tableCounter = 0;
            
            var title = html.DocumentNode.SelectSingleNode("//h2").InnerText;
            var titles = title.Split('-');
            for (var i = 0; i < titles.Length; i++) titles[i] = titles[i].Trim();
                
            foreach (var table in tables)
            {
                var hourCounter = 5;
                var tbody = table.SelectNodes(".//tbody[1]//tr");
                var dateEnum = tbody.ElementAt(tbody.Count - 1).InnerText.Where(c => Char.IsDigit(c) || c == '-');
                tbody.RemoveAt(tbody.Count - 1);
                
                var now = DateTime.Now;
                var day = now.Day < 10 ? $"0{now.Day}" : now.Day.ToString();
                var month = now.Month < 10 ? $"0{now.Month}" : now.Month.ToString();
                string date = $"{day}-{month}-{now.Year}";
                var dto = new BusTableDto() { LastUpdated = date, Direction = tableCounter};
                foreach (var tr in tbody)
                {

                    var nodes = tr.ChildNodes.Where(node => node.NodeType != HtmlNodeType.Text).ToList();
                    for (var i = 0; i < 3; i++)
                    {
                        var text = nodes[i + 1].InnerText;
                        var minutes = new List<int>();
                        if (text.Any(char.IsDigit))
                        {
                            var regex = new Regex(@"\b\d+\b");
                            foreach (Match match in regex.Matches(text))
                            {
                                minutes.Add(int.Parse(match.Value));
                            }

                            if (i == 0) dto.WorkdayArrivals[hourCounter] = minutes;
                            if (i == 1) dto.SaturdayArrivals[hourCounter] = minutes;
                            if (i == 2) dto.SundayArrivals[hourCounter] = minutes;
                        }
                    }

                    hourCounter++;
                }
                busTables.Add(dto);

                tableCounter++;
            }

            return busTables;
        }
    }
    
   

    /// <summary>
    /// Checks if the given bustable is cached, and if not caches it into the database. 
    /// </summary>
    public async Task<ICollection<BusTableDto>?> getBusTableFromWebAndCache(string name)
    {
        // convert datetime.now into a "dd-mm-yyyy" string
        var now = DateTime.Now;
        var day = now.Day < 10 ? $"0{now.Day}" : now.Day.ToString();
        var month = now.Month < 10 ? $"0{now.Month}" : now.Month.ToString();
        string date = $"{day}-{month}-{now.Year}";
        List<BusTable> busTables;
        
        busTables = ( await _busTableRepository.getBusTablesByName(name) ).ToList();
 
        if (busTables.Any())
        {
            if (busTables.ElementAt(0).LastUpdated != date)
            {
                _busTableRepository.deleteBusTablesByCollection(busTables);
                await _busTableRepository.saveChangesAsync();
            }
            else
            {
                return BusTableMapping.toDto(busTables);
            }
        }
        var htmlString = await _getStationTable(name);
        if (htmlString == null) return null;
        var dtos  = _getBusTablesFromHtml(htmlString).ToList();
        foreach (var busTableDto in dtos) busTableDto.LineNumber = name;
        var btEntities = await Mappings.BusTableMapping.toEntity(dtos, _busTableRepository);
        await _busTableRepository.addBusTableRangeAsync(btEntities);
        await _busTableRepository.saveChangesAsync();
        return dtos;

    }

    async Task<string?> _getStationTable(string id)
    {
        using(MiniProfiler.Current.Step("_getStationTable"))
        { 
            string uri = $"https://www.bgprevoz.rs/linije/red-voznje/linija/{id}/prikaz";
            // TODO: add progress output
            var resp = await _client.GetAsync(uri);
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadAsStringAsync();
        }
    }
}
