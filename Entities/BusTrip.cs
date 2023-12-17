namespace gspAPI.Entities;

public class BusTrip
{
   public int BusTripId { get; set; }
   public BusRoute BusRoute{ get; set; } = null!;
   public int BusTripDirection { get; set; }
   public string BusStripNameAlt { get; set; } = "";
   public string  BusTripName { get; set; } = null!;
   
    public IEnumerable<BusStop> BusStops{ get; set; } = null!;

}
