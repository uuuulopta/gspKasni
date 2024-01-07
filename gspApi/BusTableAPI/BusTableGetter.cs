namespace gspAPI.BusTableAPI;

using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Entities;
using Models;
using HtmlAgilityPack;
using Mappings;
using Services;

public class BusTableGetter : IBusTableGetter
{
    private readonly HttpClient _client = new();
    private readonly IBusTableRepository _busTableRepository;
    private readonly ILogger<BusTableGetter> _logger;

    public BusTableGetter(IBusTableRepository btr, ILogger<BusTableGetter> logger, HttpClient? client = null)
    {
        if (client == null)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/html"));
        }
        else _client = client;

        _busTableRepository = btr;
        _logger = logger ?? throw new ArgumentException(nameof(logger));
    }

    [SuppressMessage("ReSharper.DPA",
        "DPA0005: Database issues")]
    IEnumerable<BusTableDto> _getBusTablesFromHtml(string htmlString)
    {


        var html = new HtmlDocument();
        html.LoadHtml(htmlString);
        var tables = html.DocumentNode.SelectNodes("//table");
        if (tables == null) throw new ArgumentException("Given html page does not contain a table");
        var busTables = new List<BusTableDto>();
        var tableCounter = 0;

        var title = html.DocumentNode.SelectSingleNode("//h2").InnerText;
        var titles = title.Split('-');
        for (var i = 0; i < titles.Length; i++) titles[i] = titles[i].Trim();

        foreach (var table in tables)
        {

            var tbody = table.SelectNodes(".//tbody[1]//tr");
            tbody.RemoveAt(tbody.Count - 1);

            string date = getTodayDateFormatted();
            var dto = new BusTableDto() { LastUpdated = date, Direction = tableCounter };
            foreach (var tr in tbody)
            {

                var nodes = tr.ChildNodes.Where(node => node.NodeType != HtmlNodeType.Text).ToList();
                int hour = int.Parse(nodes[0].InnerText);
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

                        if (i == 0) dto.WorkdayArrivals[hour] = minutes;
                        if (i == 1) dto.SaturdayArrivals[hour] = minutes;
                        if (i == 2) dto.SundayArrivals[hour] = minutes;
                    }
                }

            }

            busTables.Add(dto);

            tableCounter++;
        }

        return busTables;

    }


    /// <summary>
    /// Checks if the given bustable is cached, and if not caches it into the database. 
    /// </summary>
    public async Task<ICollection<BusTableDto>?> getBusTableFromWebAndCache(string name)
    {
        // convert datetime.now into a "dd-mm-yyyy" string
        var date = getTodayDateFormatted();
        List<BusTable> busTables;
        bool updateFlag = false;

        busTables = (await _busTableRepository.getBusTablesByName(name)).ToList();

        if (busTables.Any())
        {
            if (busTables.ElementAt(0).LastUpdated != date)
            {
                updateFlag = true;
            }
            else
            {
                _logger.LogInformation($"Found {name} in cache.");
                return BusTableMapping.toDto(busTables);
            }
        }

        var htmlString = await _getStationTable(name);
        if (htmlString == null) return null;
        var dtos = _getBusTablesFromHtml(htmlString).ToList();
        foreach (var busTableDto in dtos) busTableDto.LineNumber = name;
        var btEntities = await BusTableMapping.toEntity(dtos,
            _busTableRepository);
        if (!updateFlag)
        {
            _logger.LogInformation($"Adding {name}");
            await _busTableRepository.addBusTableRangeAsync(btEntities);
        }
        else
        {
            _logger.LogInformation($"Updating {name}");
            _busTableRepository.updateBusTableRange(btEntities);
        }
        await _busTableRepository.saveChangesAsync();
        return dtos;

    }


    public async Task updateAllTables()
    {
        var lines = await _busTableRepository.getAllRoutesShortNames();
        var errored = new List<string>();
        foreach (var name in lines)
        {
            // There is no distinction between night busses on the official table website.
            if (name.EndsWith("N")) continue;
            try
            {
                await getBusTableFromWebAndCache(name);
            }
            catch (ArgumentException e)
            {
                errored.Add(name);
                _logger.LogError($"Error while updating {name}:\n {e.Message}");
            }
        }

        errored.ForEach(Console.WriteLine);
        await Task.CompletedTask;

    }

    private string getTodayDateFormatted()
    {
        var now = DateTime.Now;
        var day = now.Day < 10 ? $"0{now.Day}" : now.Day.ToString();
        var month = now.Month < 10 ? $"0{now.Month}" : now.Month.ToString();
        string date = $"{day}-{month}-{now.Year}";
        return date;
    }

    async Task<string?> _getStationTable(string id)
    {

        string uri = $"https://www.bgprevoz.rs/linije/red-voznje/linija/{id}/prikaz";

        _logger.LogInformation($"Getting {uri}");
        var resp = await _client.GetAsync(uri);
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadAsStringAsync();

    }
}
