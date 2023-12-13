namespace gspAPI.Models;

using Newtonsoft.Json;

public class BusTableDto
{
   public string LineNumber { get; set; } = "";
   public string RouteFullName { get; set; } = "";
   public string LastUpdated { get; set; } = "";
   public int Direction { get; set; }
   public Dictionary<int,ICollection<int>> WorkdayArrivals { get; set; } = new Dictionary<int, ICollection<int>>();
   public Dictionary<int,ICollection<int>> SaturdayArrivals { get; set; } = new Dictionary<int, ICollection<int>>();
   public Dictionary<int,ICollection<int>> SundayArrivals { get; set; } = new Dictionary<int, ICollection<int>>();

   public void addToArrivalsBasedOnDayId(int dayId,int key, int minuteToAdd)
   {
      if (dayId == 1)
      {
         if(WorkdayArrivals.ContainsKey(key)) WorkdayArrivals[key].Add(minuteToAdd);
         else WorkdayArrivals.Add(key,new List<int>(){minuteToAdd});
      }
      if (dayId == 2)
      {
         if(SaturdayArrivals.ContainsKey(key)) SaturdayArrivals[key].Add(minuteToAdd);
         else SaturdayArrivals.Add(key,new List<int>(){minuteToAdd});
      }
      if (dayId == 3)
      {
         if(SundayArrivals.ContainsKey(key)) SundayArrivals[key].Add(minuteToAdd);
         else SundayArrivals.Add(key,new List<int>(){minuteToAdd});
      }
          
   }
   
   
}
