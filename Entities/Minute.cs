namespace gspAPI.Entities;

public class Minute
{
 


   public int MinuteId { get; set; }
   public int Value { get; set; }
   public int DayTypeId { get; set; }
   public DayType DayType { get; set; } = null!;

   public IEnumerable<BusTable> BusTables { get; } = new List<BusTable>();
}
