namespace gspAPI.Entities;

public class PingCache
{
    public int PingCacheId { get; set; }
    public int BusTableId { get; set; }
    public BusTable BusTable { get; set; } = null!;
    
    public int TimeId { get; set; }
    public Time Time { get; set; } = null!;
    public float Lat { get; set; }
    public float Lon { get; set; }
    public float Distance { get; set; }
   
    // true if the position was found by checking the last bus in the opposite direction
    public bool GotFromOppositeDirection { get; set; } 
    public int StationsBetween { get; set; }

    public byte[] Timestamp { get; set; }

}
