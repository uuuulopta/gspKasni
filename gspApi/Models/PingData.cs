﻿namespace gspAPI.Models;

using System.ComponentModel.DataAnnotations.Schema;

public class PingData
{
    public string id { get; set; } = null!;
    public double? avg_distance { get; set; }
    public double? avg_stations_between { get; set; }
    public double? score { get; set; }


}
