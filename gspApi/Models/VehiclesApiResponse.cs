﻿// Disabling this this as it is auto-generated and handled by the json serialization 
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace gspAPI.Models;


public class VehiclesApiResponse
{
    
    public class AllStation
    {
        public int id { get; set; }
        public Coordinates coordinates { get; set; }
    }

    public class Coordinates
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        
    }

    public class Root
    {
        public int code { get; set; } = 0;
        public int seconds_left { get; set; }
        public string line_number { get; set; }
        public string? stations_gpsx { get; set; }
        public string? stations_gpsy { get; set; }
        public string station_name { get; set; }
        public string actual_line_number { get; set; }
        public int stations_between { get; set; }
        public string garage_no { get; set; }
        public string line_title { get; set; }
        public string main_line_title { get; set; }
        public List<Vehicle>? vehicles { get; set; }
        public List<AllStation> all_stations { get; set; }
        public int station_uid { get; set; }
        
        public double DistanceTo(double targetLat,double targetLon)
        {
            if (stations_gpsx == null || stations_gpsy == null) return 999;
            var baseRad = Math.PI * double.Parse(stations_gpsx )/ 180;
            var targetRad = Math.PI * targetLat/ 180;
            var theta = double.Parse(stations_gpsy) - targetLon;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            // return in km
            return dist * 1.609344;
        }

        public bool validate()
        {
            if (!double.TryParse(stations_gpsx, out _)) return false;
            if (!double.TryParse(stations_gpsy, out _)) return false;
            return true;
        }
        
    }
    
    

    public class Vehicle
    {
        public string garageNo { get; set; }
        public string? lat { get; set; }
        public string? lng { get; set; }
    }
}
