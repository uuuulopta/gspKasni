namespace gspAPI.Entities;

public class BusTable
{
   public int BusTableId { get; set; } 
   public string LineNumber { get; set; } = "";
   public int Direction { get; set; }
   public string LastUpdated { get; set; } = "";
   public IEnumerable<Time> Times { get; } = new List<Time>();
}
