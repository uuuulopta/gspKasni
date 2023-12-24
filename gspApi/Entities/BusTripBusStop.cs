namespace gspAPI.Entities;

public class BusTripBusStop
{
   public int BusTripBusStopId { get; set; }
   public BusTrip BusTrip { get; set; } 
   public BusStop BusStop { get; set; }
   
   public int BusTripId { get; set; } 
   public int BusStopId { get; set; }
   public int Direction { get; set; }
}
