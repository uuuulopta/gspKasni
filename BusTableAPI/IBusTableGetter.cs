namespace gspApiGetter.BusTableAPI;

public interface IBusTableGetter
{
    public BusTable[] getBusTableFromWeb(string id, bool checkCache = true, bool doCache = true);

}
