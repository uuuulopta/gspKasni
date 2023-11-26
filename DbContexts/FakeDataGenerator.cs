namespace gspAPI.DbContexts;
using Bogus;
using Entities;

public class FakeDataGenerator
{
    public List<Time> Times { get; set; } = new List<Time>();
    public List<BusTable> BusTables { get; set; } = new List<BusTable>();

    public List<TimeBusTable> MinuteBusTables { get; set; } = new List<TimeBusTable>();
    public void newData (int timesAmount,int busTableAmount)
    {
        int timeId = 1;
        var timeFaker = new Faker<Time>()
            .RuleFor(p => p.TimeId,
                _ => timeId++)
            .RuleFor(p => p.Minute,
                f => f.Random.Number(0, 59))
            .RuleFor(p => p.DayTypeId,
                f => f.Random.Number(1, 3)
            )
            .RuleFor(p => p.Hour,
                f => f.Random.Number(5, 23));
      
        var busTableId = 1;
        var now = DateTime.Now;
        var day = now.Day < 10 ? $"0{now.Day}" : now.Day.ToString();
        var month = now.Month < 10 ? $"0{now.Month}" : now.Month.ToString();
        string fakeDate = $"{day}-{month}-{now.Year}";
        var busTableFaker = new Faker<BusTable>()
            .RuleFor(b => b.BusTableId,
                _ => busTableId++)
            .RuleFor(b => b.LineNumber,
                f => f.Random.Word())
            .RuleFor(b => b.LastUpdated,
                _ => fakeDate)
            .RuleFor(b => b.Direction,
                _ => 0);
      
        Times = timeFaker.Generate(timesAmount);
        BusTables = busTableFaker.Generate(busTableAmount);
        var tempBusTables = new List<BusTable>();
        BusTables.ForEach(b =>
        {
            tempBusTables.Add(new BusTable()
            {
                BusTableId = b.BusTableId + BusTables.Count,
                LineNumber = b.LineNumber,
                Direction = 1,
                LastUpdated = b.LastUpdated,
            });
        });
        
        BusTables.AddRange(tempBusTables);

         
        var rand = new Random();
        foreach (var table in BusTables)
        {
            var addedIndexes = new List<int>();
            for (int i = 0; i < rand.Next(3, Times.Count - 1); i++)
            {
                int randIndex = rand.Next(1,
                    Times.Count - 1);
                if (addedIndexes.Contains(randIndex)) continue;
                addedIndexes.Add(randIndex);
                MinuteBusTables.Add(
                    new TimeBusTable()
                        { BusTableId = table.BusTableId,  TimeId= randIndex });

            }
        }
    }
}
