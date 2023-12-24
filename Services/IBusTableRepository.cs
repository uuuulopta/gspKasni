namespace gspAPI.Services;

using System.Collections;
using Entities;
using Models;

public interface IBusTableRepository
{

        public Task<IEnumerable<BusTable>> getBusTablesByName(string lineNumber);
        public void deleteBusTablesByName(string lineNumber);

        public void deleteBusTablesByCollection(ICollection<BusTable> busTables);
        public Task<(BusStop? busStop, BusRoute? busRoute)> getBusTableForeignsAsync(string routeNameShort,
                int direction);
        public void addBusTable(BusTable bt);
        public Task addBusTableRangeAsync(IEnumerable<BusTable> bt);
        public Task<List<string>> getAllRoutesShortNames();
        public Task<bool> saveChangesAsync();
        public Task<Time> getTime(int hour, int minute, int daytypeId);
        public Task<Dictionary<BusStop, List<BusTable>>> getBusTablesByTime(Time time);

        public Task<Dictionary<BusStop, List<BusTable>>> getBusTablesByTime(int hour, int minute, int daytypeid);
        public void addPingCache(PingCache pingCache);
        public Task<BusTable?> getOppositeDirectionBusTable(BusTable bt);

        public Task<IEnumerable<PingData>> getPingCacheFormattedData();

        public Task<IEnumerable<LatestPingData>>? getLatestPings();
        public Task<Time> getTimeCreateIfNone(int daytypeid, int hour, int minute);
        
        public int getCountBusTables();
        public void attach<T>(T target);
}
