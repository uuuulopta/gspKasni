namespace gspAPI.Services;

using Entities;
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
        public void addPingCache(PingCache pingCache);
        public Task<BusTable?> getOppositeDirectionBusTable(BusTable bt);

        public Task<IEnumerable<PingData>> getPingCacheFormattedData( int? from,int? to);

        public Task<IEnumerable<LatestPingData>>? getLatestPings();
        public Task<Time> getTimeCreateIfNone(int daytypeid, int hour, int minute);

        public Task addTime(Time time);
        public void updateBusTable(BusTable bt);


        public Task deleteTimeRelationshipForBusTable(int bustableId);
        
        public int getCountBusTables();

        public void attachRange(params object[] entities);
}
