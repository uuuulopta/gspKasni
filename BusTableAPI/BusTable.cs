namespace gspApiGetter.BusTableAPI;

using Newtonsoft.Json;

public enum TableDay
{
    Workday = 0,
    Saturday,
    Sunday
}
[Obsolete("Currently being integrated with the webapi in gspApi.Models")]
public class BusTable
{
    /// <summary>
    ///     Gets or sets the table data organized by day types.
    /// </summary>
    /// <remarks>
    ///     The available day types include: "Workday", "Saturday", and "Sunday".
    /// </remarks>
    ///
    
    [JsonProperty]
    internal Dictionary<TableDay, Dictionary<int, List<int>>> _TableData { get; set; } = new()
    {
        { TableDay.Workday, new Dictionary<int, List<int>>() },
        { TableDay.Saturday, new Dictionary<int, List<int>>() },
        { TableDay.Sunday, new Dictionary<int, List<int>>() }
    };


    [JsonProperty] internal string _lastUpdated { get; set; } = "";


    public static void saveObjectAsJson(object obj,string filePath = "")
    {
        try
        {
            var contentsToWriteToFile = JsonConvert.SerializeObject(obj);
            var writer = new StreamWriter(filePath);
            writer.Write(contentsToWriteToFile);
            writer.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error has occured during file creation!");
            Console.WriteLine(ex);
        }

    }

    public static BusTable[] convertTablesFromJsonString(string jsonString)
    {
        return JsonConvert.DeserializeObject<BusTable[]>(jsonString)!;

    }

    public static BusTable[] convertTablesFromJsonFile(string filePath)
    {
        var reader = new StreamReader(filePath);
        var fileContents = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<BusTable[]>(fileContents)!;
    }


    public void writeOut()
    {
       
        for (var i = 5; i <= 23; i++)
            try
            {
                Console.Write($"Hour {i}: Workday:" + string.Join(", ",
                    _TableData[TableDay.Workday][i]));
                Console.Write("\t");
                Console.Write("Saturday:" + string.Join(", ",
                    _TableData[TableDay.Saturday][i]));
                Console.Write("\t");
                Console.Write("Sunday:" + string.Join(", ",
                    _TableData[TableDay.Sunday][i]));
                Console.Write("\n");


            }
            catch (Exception e)
            {
                // ignored
            }
    }
}
