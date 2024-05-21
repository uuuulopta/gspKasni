namespace gspAPI.Entities;

public class DailyPingData
{
        
    public int DailyPingDataId { get; set; }
    public int BusRouteId { get; set; }
    public BusRoute BusRoute { get; set; } = null!;
    
    public DateTime Timestamp { get; set; }
    public double? AvgDistance { get; set; }
    public double? AvgStationsBetween { get; set; }
    public double? Score { get; set; }
}
