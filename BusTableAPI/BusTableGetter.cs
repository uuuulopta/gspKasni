namespace gspApiGetter.BusTableAPI;

using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

public class BusTableGetter
{
    readonly HttpClient _client = new();
    readonly double _cachedTimeOut;

    public BusTableGetter(HttpClient? client = null, double cachedTimeOut = 12)
    {
        if (client == null)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/html"));
            _cachedTimeOut = 12;
        }
        else _client = client;
    }

    public BusTable[] getBusTablesFromHTML(string htmlString)
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

                        busTables[tableCounter].TableData[(TableDay)i].Add(hourCounter,
                            minutes);
                    }
                }

                hourCounter++;
            }

            tableCounter++;
        }

        return busTables;
    }

    public BusTable[] getBusTableFromFile(string path)
    {
        var htmlString = File.ReadAllText(path);
        return getBusTablesFromHTML(htmlString);
    }

    public BusTable[] getBusTableFromWeb(string id, bool checkCache = true, bool doCache = true)
    {
        string path = $"{id}.BusTable.json";
        BusTable[] bust = { };
        if (checkCache && File.Exists(path))
        {
            if (DateTime.Now.Subtract(File.GetLastWriteTime(path)).TotalHours > _cachedTimeOut)
            {
                File.Delete(path);
                getBusTableFromWeb(id,
                    checkCache,
                    doCache);
            }
            else
            {
                bust = BusTable.convertTablesFromJsonFile(path);
                
            }

        }
        else
        {
            var htmlString = Task.Run(async () => await _getStationTable(id)).Result;
            bust = getBusTablesFromHTML(htmlString);
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
