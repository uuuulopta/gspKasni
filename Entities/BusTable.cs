namespace gspAPI.Entities;

public class BusTable
{
   public int BusTableId { get; set; } 
   public string LineNumber { get; set; } = "";
   public ICollection<Minute> Minutes { get; set; } = new List<Minute>();
}
