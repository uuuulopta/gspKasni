namespace gspAPI.BusTableAPI;

using Models;

public interface IBusTableGetter
{

    public Task<ICollection<BusTableDto>?> getBusTableFromWebAndCache(string name);
    public Task updateAllTables();


}
