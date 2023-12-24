namespace gspAPI.Entities;

public class Time
{
 


   
   public int TimeId { get; set; }
   public int Minute { get; set; }
   
   public int Hour { get; set; }
   public int DayTypeId { get; set; }
   public DayType DayType { get; set; } = null!;

   public IEnumerable<BusTable> BusTables { get; } = new List<BusTable>();
}
