namespace gspAPI.Mappings;

using DbContexts;
using Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using Services;

public static class BusTableMapping
{
   public static async Task<List<BusTable>> toEntity(ICollection<BusTableDto> bt,IBusTableRepository repository)
   {
      var bts = new List<BusTable>();
      foreach (var busTable in bt)
      {
        bts.Add(await BusTableMapping.toEntity(busTable,repository)); 
      }

      return bts;
   }
   public static async  Task<BusTable> toEntity(BusTableDto bt,IBusTableRepository repository)
   {
      var entity = new BusTable()
      {
         Direction = bt.Direction,
         LastUpdated = bt.LastUpdated
         
      };
      (entity.BusStop, entity.BusRoute) = await repository.getBusTableForeignsAsync(bt.LineNumber,bt.Direction);
      var times = new List<Time>(bt.WorkdayArrivals.Count+bt.SaturdayArrivals.Count+bt.SundayArrivals.Count);
      Dictionary<int,ICollection<int>>[] arrivals  = {bt.WorkdayArrivals,bt.SaturdayArrivals,bt.SundayArrivals} ;
      for (int i = 0; i < 3; i++)
      {
         foreach (var work in arrivals[i]) 
         {
            foreach (var minute in work.Value)
            {
               times.Add(new Time()
               {
                  DayTypeId = 1,
                  Hour = work.Key,
                  Minute = minute,
               
               });
            }
         }
      }

      entity.Times = times;
      
      return entity;
   }
   public static BusTableDto toDto(BusTable entity)
   {
      var dto = new BusTableDto { Direction = entity.Direction, LastUpdated = entity.LastUpdated, LineNumber = entity.BusRoute.NameShort };
      foreach (var time in entity.Times)
      {
         dto.addToArrivalsBasedOnDayId(time.DayTypeId,time.Hour,time.Minute);
      }

      return dto;
   }

   public static List<BusTableDto> toDto(IEnumerable<BusTable> entities)
   {
      
      var dtos = new List<BusTableDto>();
      foreach (var entity in entities)
      {
         dtos.Add(toDto(entity));
      }

      return dtos;
   }
}
