namespace gspAPI.Services;

using DbContexts;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
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


    public async Task<(BusStop busStop, BusRoute busRoute)> getBusTableForeignsAsync(string tripShortName,int direction)
    {
        var busRoute = await _context.BusRoutes.Where(b => b.NameShort == tripShortName).FirstOrDefaultAsync();
        if (busRoute == null)
            throw new ArgumentException($"BusRoute for tripLongName {tripShortName} could not be found.");
        // Maybe edit the BusTrip table to only contain those trips that are avalialbe in BusTripBusStop table to 
        // remove the need for a sunquery.
        var busTrip = await _context.BusTrips.Where(b => b.BusTripDirection == direction && b.BusRoute == busRoute
            && _context.BusTripBusStops.Any(bts => bts.BusTrip.BusTripId == b.BusTripId)
             )
            .FirstOrDefaultAsync();
        
        if (busTrip == null)
            throw new ArgumentException($"BusTrip for route {busRoute.NameShort} could not be found.");
        var busTripBusStop = await _context.BusTripBusStops.Where(
                          b=>
                          b.BusTrip== busTrip && b.Direction == 0
                          ).Include(b=>b.BusStop).FirstOrDefaultAsync()
                      ?? throw new ArgumentException($"BusStop for BusTrip {busTrip.BusTripName} id={busTrip.BusTripId} could not be found.");
 
            
      
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

    public async Task addBusTableAsync(BusTable bt)
    {
        await _context.BusTables.AddAsync(bt);
    }

    public async Task addBusTableRangeAsync(IEnumerable<BusTable> bt)
    {
        await _context.BusTables.AddRangeAsync(bt);
    }




    public async Task<List<BusTable>> getBusTablesByTime(Time time)
    {
        return await _context.BusTables.Where(b => b.Times.Any(t => t == time))
            .Include(b => b.Times)
            .Include(b => b.BusRoute)
            .Include(b => b.BusStop)
            .ToListAsync();
    }

    public async Task addPingCache(PingCache pingCache)
    {
        await _context.PingCaches.AddAsync(pingCache);
        
    }

    public async Task<List<BusTable>> getBusTablesByTime(int hour,int minute,int daytypeId)
    {
        return await _context.BusTables
            .Where(b => b.Times.Any(t => t.Hour == hour && t.Minute == minute && t.DayTypeId == daytypeId))
            .Include(b => b.Times)
            .Include(b => b.BusRoute)
            .Include(b => b.BusStop)
            .ToListAsync();
          
            
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
        
}
