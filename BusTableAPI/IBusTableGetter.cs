namespace gspApiGetter.BusTableAPI;

using gspAPI.Models;

public interface IBusTableGetter
{
    
    public Task<ICollection<BusTableDto>?> getBusTableFromWebAndCache(string name);

}
