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
        public Task<bool> saveChangesAsync();
}
