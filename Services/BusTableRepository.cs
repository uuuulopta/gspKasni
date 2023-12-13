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
        var busTripName = await _context.BusTrips
            .Where(b => b.BusRoute.BusRouteId == busRoute.BusRouteId && b.BusTripDirection == direction)
            .Select(b => b.BusTripName).FirstOrDefaultAsync();
            
        if (busTripName == null)
            throw new ArgumentException($"BusTrip for route {busRoute.NameShort} could not be found.");
        var busStop = await _context.BusStops.Where(b => b.BusStopName == busTripName).FirstOrDefaultAsync()
                      ?? throw new ArgumentException($"BusStop for BusTrip {busTripName} could not be found.");

            
      
        return (busStop, busRoute);
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

    

        
    public async Task<List<BusTable>> getBusTablesByTime(int hour,int minute,int daytypeId)
    {
        return await _context.BusTables
            .Where(b => b.Times.Any(t => t.Hour == hour && t.Minute == minute && t.DayTypeId == daytypeId))
            .Include(b => b.Times)
            .Include(b => b.BusRoute)
            .Include(b => b.BusStop)
            .ToListAsync();
          
            
    }
    public async Task<bool> saveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
        
}
