namespace gspApiGetter.BusTableAPI;

using gspAPI.Models;
using gspAPI.Services;

public interface IBusTableGetter
{

    public Task<ICollection<BusTableDto>?> getBusTableFromWebAndCache(string name);
    public Task updateAllTables();


}
