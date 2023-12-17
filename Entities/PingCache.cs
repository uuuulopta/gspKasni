namespace gspAPI.Entities;

public class PingCache
{
    public int PingCacheId { get; set; }
    public BusTable BusTable { get; set; } = null!;
    public Time Time { get; set; } = null!;
    public float Lat { get; set; }
    public float Lon { get; set; }
    public float Distance { get; set; }
    
    public int StationsBetween { get; set; }
    
    
}
