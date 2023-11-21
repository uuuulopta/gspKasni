namespace gspApiGetter7;

using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

public enum TableDay
{
    Workday = 0,
    Saturday,
    Sunday
}



class Program
{
    public static void Main(string[] args)
    {
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("text/html"));
        // var htmlString = Task.Run(async () => await getStationTable(client)).Result;
        // temporary cached page as not to ddos during testing
        // check out https://www.bgprevoz.rs/linije/red-voznje/linija/5/prikaz 
        var htmlString = File.ReadAllText("s.html");
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
            var bgc = 1;
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
                        foreach (Match match in regex.Matches(text)) minutes.Add(int.Parse(match.Value));

                        busTables[tableCounter].TableData[(TableDay)i].Add(hourCounter,
                            minutes);
                    }
                }
                hourCounter++;
                bgc++;
            }
            tableCounter++;
        }
        busTables[0].writeOut();
    }
    static async Task<string> getStationTable(HttpClient client)
    {
        var res = await client.GetStringAsync("https://www.bgprevoz.rs/linije/red-voznje/linija/5/prikaz");
        return res;
    }
}
