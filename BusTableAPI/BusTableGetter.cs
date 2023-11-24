namespace gspApiGetter.BusTableAPI;

using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

public class BusTableGetter : IBusTableGetter
{
    readonly HttpClient _client = new();

    public BusTableGetter(HttpClient? client = null)
    {
        if (client == null)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/html"));
        }
        else _client = client;
    }

    BusTable[] _getBusTablesFromHtml(string htmlString)
    {
        var html = new HtmlDocument();
        html.LoadHtml(htmlString);
        var tables = html.DocumentNode.SelectNodes("//table");
        // t [direction][workday/saturday/sunday][hour] -> [minutes]
        BusTable[] busTables = { new(), new() };
        var tableCounter = 0;
        foreach (var table in tables)
        {
            var hourCounter = 5;
            var tbody = table.SelectNodes(".//tbody[1]//tr");
            var dateEnum = tbody.ElementAt(tbody.Count - 1).InnerText.Where(c => Char.IsDigit(c) || c == '-' );
            string date = string.Join("", dateEnum);
            busTables[tableCounter]._lastUpdated = date;
            tbody.RemoveAt(tbody.Count - 1);
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
                            minutes.Add(int.Parse(match.Value));

                        busTables[tableCounter]._TableData[(TableDay)i].Add(hourCounter,
                            minutes);
                    }
                }

                hourCounter++;
            }

            tableCounter++;
        }

        return busTables;
    }

    BusTable[] _getBusTableFromFile(string path)
    {
        var htmlString = File.ReadAllText(path);
        return _getBusTablesFromHtml(htmlString);
    }

    public BusTable[] getBusTableFromWeb(string id, bool checkCache = true, bool doCache = true)
    {
        // convert datetime.now into a "dd-mm-yyyy" string
        var now = DateTime.Now;
        var day = now.Day < 10 ? $"0{now.Day}" : now.Day.ToString();
        var month = now.Month < 10 ? $"0{now.Month}" : now.Month.ToString();
        string date = $"{day}-{month}-{now.Year}";
        string path = $"{id}.BusTable.json";
        BusTable[] bust;
        if (checkCache && File.Exists(path))
        {
            bust = BusTable.convertTablesFromJsonFile(path);
            if ( bust[0]._lastUpdated != date || bust[1]._lastUpdated != date )
            {
                File.Delete(path);
                getBusTableFromWeb(id,
                    checkCache,
                    doCache);
            }
        }
        else
        {
            var htmlString = Task.Run(async () => await _getStationTable(id)).Result;
            bust = _getBusTablesFromHtml(htmlString);
            if(doCache) BusTable.saveObjectAsJson(bust,path); 
            
        }

        return bust;
    }

    async Task<string> _getStationTable(string id)
    {
        // TODO: add progress output
        Console.WriteLine($"Getting bus table for {id}...");
        var res = await _client.GetStringAsync($"https://www.bgprevoz.rs/linije/red-voznje/linija/{id}/prikaz");
        Console.WriteLine($"Got bus table {id}!");
        return res;
    }
}