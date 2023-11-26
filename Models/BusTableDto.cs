namespace gspAPI.Models;

public class BusTableDto
{
   public string LineNumber { get; set; } = "";
   public string LastUpdated { get; set; } = "";
   public int Hour { get; set; }
   public int Direction { get; set; }
   public IEnumerable<int> WorkdayArrivals { get; set; } = new List<int>();
   public IEnumerable<int> SaturdayArrivals { get; set; } = new List<int>();
   public IEnumerable<int> SundayArrivals { get; set; } = new List<int>();
   
}
