namespace gspAPI.Services;

using Entities;
using Models;

public interface IBusTableRepository
{

        public Task<IEnumerable<BusTable>> getBusTablesByName(string lineNumber);

        public Task<(BusStop? busStop, BusRoute? busRoute)> getBusTableForeignsAsync(string routeNameShort,
                int direction);
        public void addBusTable(BusTable bt);
        public Task addBusTableRangeAsync(IEnumerable<BusTable> bt);
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

        public void updateBusTable(BusTable bt);

        public void updateBusTableRange(IEnumerable<BusTable> bt);
        
        public int getCountBusTables();
        public void attach<T>(T target);
}
