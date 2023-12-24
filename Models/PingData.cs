namespace gspAPI.Models;

public class PingData
{
    public string id { get; set; }
    public double? avg_distance { get; set; }
    public double? avg_stations_between { get; set; }
    public double? score { get; set; }
    public byte[] time { get; set; }


}
