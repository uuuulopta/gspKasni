namespace gspApiGetter7;

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

    public void writeOut()
    {
        int[] days = { (int)TableDay.Workday, (int)TableDay.Saturday, (int)TableDay.Sunday };
        {

        }
        for (var i = 5; i <= 23; i++)
            try
            {
                Console.Write($"Hour {i}: Workday:" + string.Join(", ", TableData[TableDay.Workday][i]));
                Console.Write("\t"); Console.Write("Saturady:" + string.Join(", ",
                    TableData[TableDay.Saturday][i])); Console.Write("\t");
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
