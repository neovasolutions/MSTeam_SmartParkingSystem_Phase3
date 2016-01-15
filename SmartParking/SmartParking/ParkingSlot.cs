using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SmartParking
{
    public enum ParkingStatus
    {
        InActive = 0,
        Free = 1,
        Parked = 2,
        AdvParked = 3
    }
    public  class ParkingSlot: System.Windows.Controls.Button    
    {
        private ParkingStatus _ParkStatus;
        //private bool _Parked = false;
        //private bool _IsActive = true;
        public Nullable <int> ParkedBy { set; get; }
        public ParkingStatus ParkStatus
        { 
            set{
                if (_ParkStatus == ParkingStatus.InActive) 
                    return; 
                _ParkStatus = value;
                switch (_ParkStatus)
                {
                    case  ParkingStatus.Parked :
                        Background = new SolidColorBrush(Colors.Green);
                        break;
                    case ParkingStatus.AdvParked:
                        Background = new SolidColorBrush(Colors.Blue);
                        break;
                    case ParkingStatus.Free :
                        Background = new SolidColorBrush(Colors.White);
                        break;
                    case ParkingStatus.InActive :
                        Background = new SolidColorBrush(Colors.Gray);
                        break;
                }
            }
            get
            {
                return _ParkStatus;
            }
        }
        public void MakeMeFree()
        {
            Background = new SolidColorBrush(Colors.White);
            _ParkStatus = ParkingStatus.Free;
        }
        public void ParkByMe()
        {
            Background = new SolidColorBrush(Colors.Orange);
        }
        public void AdvParkByMe()
        {
            Background = new SolidColorBrush(Colors.Purple);
        }
    }
}
