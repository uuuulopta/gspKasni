namespace gspAPI.Entities;

public class BusTable
{
   public int BusTableId { get; set; } 
   public string LineNumber { get; set; } = "";
   public IEnumerable<Minute> Minutes { get; } = new List<Minute>();
}
