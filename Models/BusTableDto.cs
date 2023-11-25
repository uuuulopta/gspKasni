namespace gspAPI.Models;

public class BusTableDto
{
   public string LineNumber { get; set; } = "";
   public IEnumerable<int> WorkdayArrivals { get; set; } = new List<int>();
   public IEnumerable<int> SaturdayArrivals { get; set; } = new List<int>();
   public IEnumerable<int> SundayArrivals { get; set; } = new List<int>();
   
}
