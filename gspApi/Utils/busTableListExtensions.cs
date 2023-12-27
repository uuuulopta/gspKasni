namespace gspAPI.Utils;

using Entities;

public static class BusTableListExtensions
{
    public static IEnumerable<string> getRouteNameShort(this List<BusTable> bts)
    {
         return bts.Select(b => b.BusRoute.NameShort);
    } 
            
}
