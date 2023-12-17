namespace gspAPI.Services;

using Entities;

public interface IBusTableRepository
{

        public Task<IEnumerable<BusTable>> getBusTablesByName(string lineNumber);
        public void deleteBusTablesByName(string lineNumber);

        public void deleteBusTablesByCollection(ICollection<BusTable> busTables);
        public Task<(BusStop busStop, BusRoute busRoute)> getBusTableForeignsAsync(string tripLongName, int direction);
        public Task addBusTableAsync(BusTable bt);
        public Task addBusTableRangeAsync(IEnumerable<BusTable> bt);
        public Task<List<BusTable>> getBusTablesByTime(int hour,int minute,int daytypeId);
        public Task<List<string>> getAllRoutesShortNames();
        public Task<bool> saveChangesAsync();
        public Task<Time> getTime(int hour, int minute, int daytypeId);
        public Task<List<BusTable>> getBusTablesByTime(Time time);
        public Task addPingCache(PingCache pingCache);
}
