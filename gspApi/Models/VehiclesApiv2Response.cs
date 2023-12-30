// Disabling this because as it is auto-generated and handled by the json serialization 
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace gspAPI.Models;

public class VehiclesApiv2Response
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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

    public class Data
    {
        public int just_coordinates = 0;
        public int seconds_left { get; set; }
        public string line_number { get; set; }
        public string station_name { get; set; }
        public string id { get; set; }
        public string actual_line_number { get; set; }
        public int stations_between { get; set; }
        public string garage_no { get; set; }
        public string line_title { get; set; }
        public string main_line_title { get; set; }
        public List<Vehicle> vehicles { get; set; }
        public List<AllStation> all_stations { get; set; }
        public int station_uid { get; set; }
        public double DistanceTo(double targetLat,double targetLon)
        {
            var baseRad = Math.PI * double.Parse(vehicles[0].lat)/ 180;
            var targetRad = Math.PI * targetLat/ 180;
            var theta = double.Parse(vehicles[0].lng) - targetLon;
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
    }

    public class Root
    {
        public bool success { get; set; }
        public int code { get; set; }
        public List<Data> data { get; set; }
        
    }

    public class Vehicle
    {
        public string garageNo { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }


}
