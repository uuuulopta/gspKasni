﻿namespace gspApiGetter.BusTableAPI;

using Newtonsoft.Json;

public enum TableDay
{
    Workday = 0,
    Saturday,
    Sunday
}

public class BusTable
{
    /// <summary>
    ///     Gets or sets the table data organized by day types.
    /// </summary>
    /// <remarks>
    ///     The available day types include: "Workday", "Saturday", and "Sunday".
    /// </remarks>
    public Dictionary<TableDay, Dictionary<int, List<int>>> TableData { get; set; } = new()
    {
        { TableDay.Workday, new Dictionary<int, List<int>>() },
        { TableDay.Saturday, new Dictionary<int, List<int>>() },
        { TableDay.Sunday, new Dictionary<int, List<int>>() }
    };


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
                    TableData[TableDay.Workday][i]));
                Console.Write("\t");
                Console.Write("Saturday:" + string.Join(", ",
                    TableData[TableDay.Saturday][i]));
                Console.Write("\t");
                Console.Write("Sunday:" + string.Join(", ",
                    TableData[TableDay.Sunday][i]));
                Console.Write("\n");


            }
            catch (Exception e)
            {
                // ignored
            }
    }
}