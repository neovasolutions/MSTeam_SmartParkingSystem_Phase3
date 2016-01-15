using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SmartParking
{
    public partial class Parking_Pentagon : ParkingBase
    {
        public Parking_Pentagon()
        {
            InitializeComponent();
            InitMe(ContentPanel, ParkCommand, ParkHeader);
        }
        protected override void ClearAll_ParkingSlot()
        {
            ((ParkingSlot)ContentPanel.FindName("R0C0")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R0C1")).MakeMeFree();
            
            ((ParkingSlot)ContentPanel.FindName("R1C1")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R2C1")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R3C1")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R4C1")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R5C1")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R6C1")).MakeMeFree();
            
            ((ParkingSlot)ContentPanel.FindName("R1C2")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R2C2")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R3C2")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R4C2")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R5C2")).MakeMeFree();
            ((ParkingSlot)ContentPanel.FindName("R6C2")).MakeMeFree();
        }
        public void ParkSlot_Click(object sender, RoutedEventArgs e)
        {
            ParkSlot_Click(sender);
        }
    }
}