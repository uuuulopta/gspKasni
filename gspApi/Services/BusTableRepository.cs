namespace gspAPI.Services;

using System.Runtime.CompilerServices;
using DbContexts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Models;


public class BusTableRepository : IBusTableRepository
{
    
    private readonly MysqlContext _context;

    public BusTableRepository(MysqlContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }


    /// <summary>
    /// Should always return 2 or null
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<BusTable>> getBusTablesByName(string lineNumber)
    {

        return await _context.BusTables.Where(b => b.BusRoute.NameShort == lineNumber)
            .Include(b => b.Times)
            .Include(b => b.BusRoute)
            .Include(b => b.BusStop)
            .ToListAsync();
    }


    public async Task<(BusStop? busStop, BusRoute? busRoute)> getBusTableForeignsAsync(string routeNameShort,int direction)
    {
        var busRoute = await _context.BusRoutes.Where(b => b.NameShort == routeNameShort).FirstOrDefaultAsync();
        if (busRoute == null) return (null, null);
        
        var busTrip = await _context.BusTrips.Where(b => b.BusTripDirection == direction && b.BusRoute.BusRouteId == busRoute.BusRouteId
                && _context.BusTripBusStops.Any(bts => bts.BusTrip.BusTripId == b.BusTripId)
            )
            .FirstOrDefaultAsync();
        
        if (busTrip == null) return (null, null);
        
        var busTripBusStop = await _context.BusTripBusStops.Where(
            b =>
                b.BusTrip == busTrip && b.Direction == 0
        ).Include(b => b.BusStop).FirstOrDefaultAsync();
 
        if (busTripBusStop == null) return (null, null);
      
        return (busTripBusStop.BusStop, busRoute);
    }
    public void deleteBusTablesByName(string lineNumber)
    {
        _context.BusTables.RemoveRange(_context.BusTables.Where(b => b.BusRoute.NameShort == lineNumber).ToList());
    }

    public void deleteBusTablesByCollection(ICollection<BusTable> busTables)
    {
        _context.BusTables.RemoveRange(busTables); 
    }

    public void addBusTable(BusTable bt)
    {
        _context.BusTables.Add(bt);
    }

    public async Task addBusTableRangeAsync(IEnumerable<BusTable> bt)
    {
        await _context.BusTables.AddRangeAsync(bt);
    }




    public async Task<Dictionary<BusStop,List<BusTable>>> getBusTablesByTime(Time time)
    {
        return await _context.BusTables.Where(b => b.Times.Any(t => t.TimeId == time.TimeId))
            .Include(b => b.Times)
            .Include(b => b.BusRoute)
            .Include(b => b.BusStop)
            .GroupBy(b => b.BusStop)
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // I like to check it just to ease my mind
            .Where(group => group.Key != null && group.Any())
            .Select(group => new
            {
                BusStop = group.Key,
                BusTables = group.ToList()
            })
            .ToDictionaryAsync(
                item => item.BusStop,
                item => item.BusTables
            );
    }

    
    public async Task<Dictionary<BusStop,List<BusTable>>> getBusTablesByTime(int hour, int minute, int daytypeid)
    {
        return await _context.BusTables.Where(b => b.Times.Any(t =>
                t.Hour == hour && t.Minute == minute && t.DayTypeId == daytypeid))
            .Include(b => b.Times)
            .Include(b => b.BusRoute)
            .Include(b => b.BusStop)
            .GroupBy(b => b.BusStop)
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // I like to check it just to ease my mind
            .Where(group => group.Key != null && group.Any())
            .Select(group => new
            {
                BusStop = group.Key,
                BusTables = group.ToList()
            })
            .ToDictionaryAsync(
                item => item.BusStop,
                item => item.BusTables
            );
    }
    public  void addPingCache(PingCache pingCache)
    {
        _context.PingCaches.Add(pingCache);
        
    }

    public async Task<BusTable?> getOppositeDirectionBusTable(BusTable bt)
    {
        return await _context.BusTables
            .Where(b => b.Direction != bt.Direction
                        && b.BusRoute == bt.BusRoute
            )
            .Include(b => b.Times)
            .Include(b => b.BusRoute)
            .Include(b => b.BusStop)
            .FirstOrDefaultAsync();
    }


    public async Task<Time?> getTime(int hour, int minute, int daytypeId)
    {
        return await _context.Times.Where(t => t.Hour == hour && t.Minute == minute && t.DayTypeId == daytypeId)
            .FirstOrDefaultAsync();
    }

    
    public async Task<List<string>> getAllRoutesShortNames()
    {
        return await _context.BusRoutes.Select(br => br.NameShort).ToListAsync();
    }

    public async Task<bool> saveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }

    public async Task<IEnumerable<PingData>> getPingCacheFormattedData(int? from, int? to)
    {
        string fromS = "1=1";
        string toS = "1=1";
        if(from != null) fromS = $"PingCaches.Timestamp >= STR_TO_DATE({from.ToString()},'%Y%m%d')";
        if(to != null) toS = $"PingCaches.Timestamp <= STR_TO_DATE({to.ToString()},'%Y%m%d')";
        var query = @$"
        SELECT 
        BusRoutes.NameShort AS id,
            AVG(CASE WHEN Distance <> 999 THEN Distance ELSE NULL END) AS avg_distance,
            AVG(CASE WHEN StationsBetween <> 999 THEN StationsBetween ELSE NULL END) AS avg_stations_between,
            SUM(CASE WHEN DISTANCE <= 2 THEN 1 ELSE 0 END) / COUNT(BusRouteId) AS score
        FROM  PingCaches JOIN BusTables USING (BusTableId) JOIN BusRoutes USING (BusRouteId)
        WHERE {fromS} AND {toS}
        GROUP BY BusRouteId; ";
        return await _context.PingData.FromSql(FormattableStringFactory.Create(query)).ToListAsync();
    }

    public async Task<IEnumerable<LatestPingData>> getLatestPings()
    {
        return await _context.PingCaches.OrderByDescending(p => p.PingCacheId).Take(25)
            .Select(row => new LatestPingData()
            {
                id = row.BusTable.BusRoute.NameShort,
                distance = row.Distance,
                lat = row.Lat,
                lon = row.Lon,
                stations_between = row.StationsBetween
            } ).ToListAsync();
    }

    public async Task<Time> getTimeCreateIfNone(int daytypeid, int hour, int minute)
    {
        var time=await _context.Times.Where(t => t.DayTypeId == daytypeid && t.Hour == hour && t.Minute == minute)
            .FirstOrDefaultAsync();
        if (time != null) return time;
        else return new Time()
        {
            DayTypeId = daytypeid,
            Hour = hour,
            Minute = minute,
        };

    }

    public int getCountBusTables()
    {
        return _context.BusTables.Count();
    }

    public  void attach<T>(T target)
    {
        if (target != null) _context.Attach(target);
        else throw new ArgumentNullException(nameof(target),"Tried to attach on a null target");
    }
        
}
