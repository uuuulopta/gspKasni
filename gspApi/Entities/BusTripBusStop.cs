﻿namespace gspAPI.Entities;

public class BusTripBusStop
{
   public int BusTripBusStopId { get; set; }
   public BusTrip BusTrip { get; set; } = null!;
   public BusStop BusStop { get; set; } = null!;
   
   public int BusTripId { get; set; } 
   public int BusStopId { get; set; }
   public int Direction { get; set; }
}
