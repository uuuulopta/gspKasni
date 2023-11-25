namespace gspAPI.DbContexts;
using Bogus;
using Entities;

public class FakeDataGenerator
{
    public List<Minute> Minutes { get; set; } = new List<Minute>();
    public List<BusTable> BusTables { get; set; } = new List<BusTable>();

    public List<MinuteBusTable> MinuteBusTables { get; set; } = new List<MinuteBusTable>();
    public void newData (int minutesAmount,int busTableAmount)
    {
        int minuteId = 1;
        var minuteFaker = new Faker<Minute>()
            .RuleFor(p => p.MinuteId,
                _ => minuteId++)
            .RuleFor(p => p.Value,
                f => f.Random.Number(0, 59))
            .RuleFor(p => p.DayTypeId,
                f => f.Random.Number(1, 3)
            );
      
        var busTableId = 1;
        var busTableFaker = new Faker<BusTable>()
            .RuleFor(b => b.BusTableId,
                _ => busTableId++)
            .RuleFor(b => b.LineNumber,
                f => f.Random.Word());
      
         Minutes = minuteFaker.Generate(minutesAmount);
         BusTables = busTableFaker.Generate(busTableAmount);

         
         var rand = new Random();
         foreach (var table in BusTables)
         {
             var addedIndexes = new List<int>();
             for (int i = 0; i < rand.Next(3, Minutes.Count - 1); i++)
             {
                 int randIndex = rand.Next(1,
                     Minutes.Count - 1);
                 if (addedIndexes.Contains(randIndex)) continue;
                 addedIndexes.Add(randIndex);
                 MinuteBusTables.Add(
                     new MinuteBusTable()
                         { BusTableId = table.BusTableId, MinuteId = randIndex });

             }
         }
    }
}
