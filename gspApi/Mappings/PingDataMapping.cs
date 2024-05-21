namespace gspAPI.Mappings;

using Entities;
using Models;

public class PingDataMapping
{
    public static IEnumerable<PingData> fromDailyPing(ICollection<DailyPingData> dpd)
    {
        var pd = new List<PingData>();
        foreach (var p in dpd)
        {
           pd.Add(new PingData
           {
               id = p.BusRoute.NameShort,
               avg_distance = p.AvgDistance,
               avg_stations_between = p.AvgStationsBetween,
               score = p.Score
           }); 
        }

        return pd;

    }
}
