using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows;
using System.Windows.Controls;
using System.IO.IsolatedStorage;
using SmartParking.API.Models;

namespace SmartParking
{
    public enum ParkingMode
    {
        Regular=0,
        AdvancePark=1
    }
    public delegate void ActionOnImgTap();
    public class ParkingBase: PhoneApplicationPage
    {
        private Grid _SlotContainer;
        private UCParkCommand _ParkCommand;
        private UCParkHeader _ParkHeader;
        private static readonly string _SelParking = "SmartParking_SelParking";
        public static readonly string C_UserID = "SmartParking_UserIdentity";
        protected ParkingMode _ParkMode;
        private ParkingSlot _MyParkSlot;
        public ParkingBase()
        {
            this.Loaded += ParkingBase_Loaded;
        }
        public static string  UserID
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(C_UserID))
                    return IsolatedStorageSettings.ApplicationSettings[C_UserID].ToString();
                else
                    return null;
            }
        }
        void ParkingBase_Loaded(object sender, RoutedEventArgs e)
        {
            _ParkMode = (ParkingMode)System.Convert.ToInt32(NavigationContext.QueryString["ParkingMode"]);
            SetParkMode(_ParkMode);
            imgRefresh_Tap();
            Camera.ActionAfterBCScanned = new TakeAction(AllocateParkingSlots);
        }
        public void InitMe(Grid slotContnr, UCParkCommand ParkCommand, UCParkHeader ParkHeader)
        {
            _SlotContainer = slotContnr;
            _ParkCommand = ParkCommand;
            _ParkHeader = ParkHeader;
            _ParkCommand.Home_Tap = new ActionOnImgTap(imgHome_Tap);
            _ParkCommand.Park_Tap = new ActionOnImgTap(imgPark_Tap);
            _ParkCommand.UnPark_Tap  = new ActionOnImgTap(imgUnPark_Tap );
            _ParkCommand.UserProf_Tap = new ActionOnImgTap(imgUsrProf_Tap );
            _ParkHeader.Refresh_Tap = new ActionOnImgTap(imgRefresh_Tap);
        }
        private void SetParkMode(ParkingMode ParkMode)
        {
            _ParkMode = ParkMode;
            if (_ParkMode == ParkingMode.AdvancePark)
                _ParkCommand.SetMeInAdvanceParkMode();
            else
                _ParkCommand.SetMeInRegularParkMode();
            _ParkHeader.ParkMode = _ParkMode;
        }
        public void LoadParkingSlots()
        {
            WebAPICaller myWebAPICaller = new WebAPICaller();
            myWebAPICaller.JsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(SlotModel[]));
            myWebAPICaller.ActionOnReadCompleted = new DlgtActionOnReadCompleted(ActionOn_LoadParkingSlots);
            string selParking = IsolatedStorageSettings.ApplicationSettings[_SelParking].ToString();
            myWebAPICaller.GET("SlotMaster/GetSlots" , "id=" + selParking);
        }
        public static void NavigateToParking(System.Windows.Navigation.NavigationService NavgServ, ParkingMode ParkMode)
        {
            string selParking = IsolatedStorageSettings.ApplicationSettings[_SelParking].ToString();
            switch (selParking)
            {
                case "1":
                    NavgServ.Navigate(new Uri("/Parking_Pentagon.xaml?ParkingMode=" + ((int)ParkMode).ToString (), UriKind.Relative));
                    break;
                case "2":
                    NavgServ.Navigate(new Uri("/Parking_Amanora.xaml?ParkingMode=" + ((int)ParkMode).ToString(), UriKind.Relative));
                    break;
                default:
                    MessageBox.Show("App is not configured for selected Parking");
                    break;
            }
        }
        void ActionOn_LoadParkingSlots(object myList)
        {
            if (myList == null) return;
            SlotModel[] parkedSlot = (SlotModel[])myList;
            ParkingSlot slot;
            ClearAll_ParkingSlot();
            foreach (SlotModel item in parkedSlot)
            {
                slot = (ParkingSlot)_SlotContainer.FindName(item.SlotNumber);
                if (slot == null)
                    continue;
                else
                {
                    slot.ParkStatus = (ParkingStatus)item.SlotStatus;
                    slot.ParkedBy = item.CurrentUserID;
                    if (item.CurrentUserID.ToString() == UserID)
                    {
                        if (slot.ParkStatus == ParkingStatus.Parked)
                            slot.ParkByMe();
                        if (slot.ParkStatus == ParkingStatus.AdvParked)
                            slot.AdvParkByMe();
                    }
                }
            }
        }
        protected virtual void ClearAll_ParkingSlot()
        {
            
        }
        private void AllocateParkingSlots(bool isCalledFromBarcodeScanner, string barcodeNo, ParkingStatus parkStatus)
        {
            WebAPICaller myWebAPICaller = new WebAPICaller();
            string selParking = IsolatedStorageSettings.ApplicationSettings[_SelParking].ToString();
            
            string urlParam = "userId=" + UserID + "&parkingId=" + selParking + "&slotNo=" + barcodeNo + "&SlotStatus=" + ((int)parkStatus).ToString();
            myWebAPICaller.POST_urlParam("SlotMaster/PostParkUnPark",  urlParam );
            if (isCalledFromBarcodeScanner)
            {
                object slot = _SlotContainer.FindName(barcodeNo);
                if (slot == null) return;
                ((ParkingSlot)slot).ParkStatus = parkStatus;
            }
        }
        
        private void ProcessAdvParking(ParkingSlot parkSlot)
        {
            if (parkSlot.ParkStatus == ParkingStatus.Parked)
            {
                MessageBox.Show("Parked Slots are not allowed for Advance Parking");
                return;
            }
            else
            {
                if (MessageBox.Show("Are you Confirmed", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
                else
                {
                    if (parkSlot.ParkStatus == ParkingStatus.AdvParked)
                    {
                        parkSlot.ParkStatus = ParkingStatus.Free;
                        AllocateParkingSlots(false, parkSlot.Name, parkSlot.ParkStatus);
                    }
                    else
                    {
                        WebAPICaller myWebAPICaller = new WebAPICaller();
                        myWebAPICaller.ActionOnReadCompleted = new DlgtActionOnReadCompleted(ActionOnReadCompleted_BalAmount);
                        myWebAPICaller.JsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(int));
                        myWebAPICaller.GET("UserProfile/GetWalletAmount?userID=" + UserID);
                        _MyParkSlot = parkSlot;
                    }
                }
            }
        }
        void ActionOnReadCompleted_BalAmount(object amount)
        {
            if (amount == null)
            {
                MessageBox.Show("No enough balance Amount in your Wallet.");
                return;
            }
            if (System.Convert.ToInt32(amount) < 20)
            {
                MessageBox.Show("No enough balance Amount in your Wallet.");
                return;
            }
            else
            {
                _MyParkSlot.ParkStatus = ParkingStatus.AdvParked;
                _MyParkSlot.ParkedBy = System.Convert.ToInt32(UserID);
                _MyParkSlot.AdvParkByMe();
                AllocateParkingSlots(false, _MyParkSlot.Name, _MyParkSlot.ParkStatus);
            }
        }
        private void ProcessRegParking(ParkingSlot parkSlot)
        {
            if (parkSlot.ParkStatus == ParkingStatus.Free || parkSlot.ParkStatus == ParkingStatus.AdvParked)
            {
                parkSlot.ParkedBy = System.Convert.ToInt32(UserID);
                parkSlot.ParkStatus = ParkingStatus.Parked;
                parkSlot.ParkByMe();
            }
            else
            {
                parkSlot.ParkedBy = -1;
                parkSlot.ParkStatus = ParkingStatus.Free;
            }
            AllocateParkingSlots(false, parkSlot.Name, parkSlot.ParkStatus);
        }
        public void ParkSlot_Click(object sender)
        {
            ParkingSlot parkSlot = (ParkingSlot)sender;
            if (parkSlot.ParkStatus == ParkingStatus.InActive) return;
            if ((parkSlot.ParkStatus == ParkingStatus.Parked || parkSlot.ParkStatus == ParkingStatus.AdvParked) && parkSlot.ParkedBy.ToString() != UserID)
            {
                MessageBox.Show("This slot is allocated by other User");
                return;
            }
            if (_ParkMode == ParkingMode.AdvancePark)
                ProcessAdvParking(parkSlot);
            else
                ProcessRegParking(parkSlot);
        }
        public void imgRefresh_Tap()
        {
            ClearAll_ParkingSlot();
            LoadParkingSlots();
        }
        public void imgHome_Tap()
        {
            NavigationService.Navigate(new Uri("/ParkingSel.xaml", UriKind.Relative));
        }
        public void imgPark_Tap()
        {
            Camera.IsForParked = ParkingStatus.Parked;
            NavigationService.Navigate(new Uri("/BarcodeScanner.xaml", UriKind.Relative));
        }
        public void imgUnPark_Tap()
        {
            Camera.IsForParked =  ParkingStatus.Free;
            NavigationService.Navigate(new Uri("/BarcodeScanner.xaml", UriKind.Relative));
        }
        public void imgUsrProf_Tap()
        {
            NavigationService.Navigate(new Uri("/UserProfile.xaml", UriKind.Relative));
        }
    }
}
