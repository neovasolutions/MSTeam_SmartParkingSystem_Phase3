using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartParkingSystemAPI.Models
{
    public class ParkingSearchModel
    {
        public string ParkingName { get; set; }
        public string ParkingAddress { get; set; }
        public decimal? Longitute { get; set; }
        public decimal? Latitude { get; set; }
    }
}