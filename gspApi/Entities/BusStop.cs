namespace gspAPI.Entities;

public class BusStop
{
    public int BusStopId;
    public double Lat;
    public double Lon;
    public string BusStopName = null!;
    public IEnumerable<BusTrip> BusTrips{ get; set; } = null!;
}
