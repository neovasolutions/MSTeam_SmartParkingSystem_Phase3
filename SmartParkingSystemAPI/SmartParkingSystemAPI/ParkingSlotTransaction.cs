//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartParkingSystemAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParkingSlotTransaction
    {
        public int TransactionID { get; set; }
        public int SlotID { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> ParkingInTime { get; set; }
        public Nullable<System.DateTime> ParkingOutTime { get; set; }
        public string VehicleNumber { get; set; }
    
        public virtual SlotMaster SlotMaster { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}