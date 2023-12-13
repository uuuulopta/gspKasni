namespace gspAPI.Entities;

public class BusTable
{
   public int BusTableId { get; set; } 
   public int Direction { get; set; }
   public string LastUpdated { get; set; } = "";

   public BusRoute BusRoute { get; set; } = null!;
   public BusStop BusStop { get; set; } = null!;
   public IEnumerable<Time> Times { set;get; } = new List<Time>();
   
}
