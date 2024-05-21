namespace gspAPI.Services;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;

public interface IBusTableRepository
{

        
    public Task<IEnumerable<BusTable>> getBusTablesByName(string lineNumber);
    public Task<int?> getBusTableIdByColumns(int busStopId,int busRouteId,int direction);
        
    public Task<(BusStop? busStop, BusRoute? busRoute)> getBusTableForeignsAsync(string routeNameShort,
        int direction);
    public Task<List<string>> getAllRoutesShortNames();
    public Task<bool> saveChangesAsync();
    public Task<Time?> getTime(int hour, int minute, int daytypeId);
    public Task<Dictionary<BusStop, List<BusTable>>> getBusTablesByTime(Time time);

    public Task<Dictionary<BusStop, List<BusTable>>> getBusTablesByTime(int hour, int minute, int daytypeid);
    public Task<BusTable?> getOppositeDirectionBusTable(BusTable bt);

    public Task<IEnumerable<PingData>> getPingCacheFormattedData( int? from,int? to);
    
    public Task<List<PingData>> getPingCacheFormattedDataFromDaily(int? from, int? to);

    public Task<IEnumerable<LatestPingData>>? getLatestPings();
    public Task<Time> getTimeCreateIfNone(int daytypeid, int hour, int minute);

    public Task addTime(Time time);
    
    public void addPingCache(PingCache pingCache);
    public Task addDailyPingDataRangeAsync(ICollection<DailyPingData> dpd);
    public void updateBusTable(BusTable bt);


    public Task deleteTimeRelationshipForBusTable(int bustableId);
        
    public int getCountBusTables();

    public void attachRange(params object[] entities);

    /// <summary>
    /// Checks if a Table of a given entity is empty 
    /// </summary>
    /// <returns></returns>
    public Task<bool> isTableEmptyAsync<Entity>() where Entity : class;

    /// <summary>
    /// Only checks Year Day and Month.
    /// Seconds, Milliseconds, etc are irrelevant
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public Task<bool> existsDailyPingDataForDate(DateTime date);
    public DateTime getOldestPingCacheDate();
    public DateTime getNewestPingCacheDate();
}
