using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SmartParkingSystemAPI.Models
{
    public class ParkingModel
    {
        public int ParkingID { get; set; }
        [DisplayName("Parking Name")]
        public string ParkingName { get; set; }
        [DisplayName("Parking Address")]
        public string ParkingAddress { get; set; }
        [DisplayName("Parking Template Name")]
        public string ParkingTemplateName { get; set; }
        [DisplayName("Is Active")]
        public Nullable<bool> IsActive { get; set; }
        [DisplayName("No Of Slots")]
        public int NumberOfSlots { get; set; }
        [DisplayName("Min Latitude")]
        public Nullable<decimal> Min_Latitude { get; set; }
        [DisplayName("Max Latitude")]
        public Nullable<decimal> Max_Latitude { get; set; }
        [DisplayName("Min Longitude")]
        public Nullable<decimal> Min_Longitude { get; set; }
        [DisplayName("Max Longitude")]
        public Nullable<decimal> Max_Longitude { get; set; }
    }
}