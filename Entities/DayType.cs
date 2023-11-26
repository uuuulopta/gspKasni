namespace gspAPI.Entities;

public class DayType
{
  

    public int DayTypeId { get; set; }
    public string Name{ get; set; }

    public IEnumerable<Time> Minutes { get; set; } = new List<Time>();


    public DayType(string name)
    {
        Name = name;
    }
}
