namespace gspAPI.Services;

public interface IBusTableRepository
{
    Task<string> getLastUpdatedByName(string LineNumber);
}
