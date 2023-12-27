namespace gspAPI.Services;

using System.Collections;
using DbContexts;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Models;
using StackExchange.Profiling;


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


    public async Task<Time> getTime(int hour, int minute, int daytypeId)
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

    public async Task<IEnumerable<PingData>> getPingCacheFormattedData()
    {
        return await _context.PingCaches
            .Join(_context.BusTables, pc => pc.BusTableId, bt => bt.BusTableId, (pc, bt) => new { pc, bt })
            .Join(_context.BusRoutes, combined => combined.bt.BusRoute.BusRouteId, br => br.BusRouteId, (combined, br) => new { combined.pc, combined.bt, br })
            .GroupBy(combined => combined.br.BusRouteId)
            .Select(grouped => new PingData()
            {
                id = grouped.FirstOrDefault().br.NameShort,
                avg_distance = grouped.SelectMany(g => g.bt.PingCaches)
                    .Where(pc => pc.Distance != null && pc.Distance != 999)
                    .Average(pc => (double?)pc.Distance),
                avg_stations_between = grouped.SelectMany(g => g.bt.PingCaches)
                    .Where(pc => pc.StationsBetween != null && pc.StationsBetween != 999)
                    .Average(pc => (double?)pc.StationsBetween),
                score = grouped.SelectMany(g => g.bt.PingCaches)
                    .Count(pc => pc.Distance <= 2) / (double)grouped.SelectMany(g => g.bt.PingCaches).Count(),
                time = grouped.FirstOrDefault().pc.Timestamp
            })
            .ToListAsync();
        // select new
        // {
        //     id = grouped.FirstOrDefault().br.NameShort,
        //     time = grouped.FirstOrDefault().pc.Timestamp,
        //     avg_distance = grouped.SelectMany(g => g.bt.PingCaches).Where(pc => pc.Distance != 1000)
        //         .Average(pc => pc.Distance),
        //     avg_stations_between = grouped.SelectMany(g => g.bt.PingCaches).Where(pc => pc.StationsBetween != 1000)
        //         .Average(pc => pc.StationsBetween),
        //     score = grouped.SelectMany(g => g.bt.PingCaches).Count(pc => pc.Distance <= 3) / (double)grouped.Count()
        // };



    }

    public async Task<IEnumerable<LatestPingData>>? getLatestPings()
    {
        return await _context.PingCaches.OrderByDescending(p => p.PingCacheId).Take(25)
            .Select(row => new LatestPingData()
            {
                id = row.BusTable.BusRoute.NameShort,
                distance = row.Distance,
                lat = row.Lat,
                lon = row.Lon,
                stations_between = row.GotFromOppositeDirection == false ? row.StationsBetween : -1
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
        _context.Attach(target);
    }
        
}
