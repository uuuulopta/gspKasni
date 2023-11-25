namespace gspAPI.Entities;

public class DayType
{
  

    public int DayTypeId { get; set; }
    public string Name{ get; set; }

    public IEnumerable<Minute> Minutes { get; set; } = new List<Minute>();


    public DayType(string name)
    {
        Name = name;
    }
}
