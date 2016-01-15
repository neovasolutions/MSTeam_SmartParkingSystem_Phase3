using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SmartParking.API.Models;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SmartParking
{
    public partial class ParkingSel : PhoneApplicationPage
    {
        private readonly string _SelParking = "SmartParking_SelParking";
        
        public ParkingSel()
        {
            InitializeComponent();
            ParkCommand.SetMeInParkSelMode();
            ParkCommand.Wallet_Tap = new ActionOnImgTap(imgWallet_Tap);
            ParkCommand.UserProf_Tap = new ActionOnImgTap(imgUsrProf_Tap);
            ParkCommand.Park_Tap = new ActionOnImgTap(imgContinue_Tap);
            ParkCommand.AdvPark_Tap  = new ActionOnImgTap(imgAdvPark_Tap);
        }
        private void LoadParkingList()
        {
            WebAPICaller myWebAPICaller = new WebAPICaller();
            myWebAPICaller.JsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(SelectedListModel[]));
            myWebAPICaller.ActionOnReadCompleted = new DlgtActionOnReadCompleted(ActionOnReadCompleted);
            myWebAPICaller.GET("parkingmaster/GetParkings/");
        }
        void ActionOnReadCompleted(object myList)
        {
            if (myList == null) return;
            SelectedListModel[] EntityList;
            EntityList = (SelectedListModel[])myList;
            List<SelectedListModel> lst = new List<SelectedListModel>();
            //listPckPhase.Items.Clear();
            lst.AddRange(EntityList);
            lstParking.ItemsSource = lst;
            SelectParking_ByGeoLoc();
        }
        private void SelectParkingAuto(string Latitude, string Longitude)
        {
            Latitude = "2"; Longitude = "5";
            WebAPICaller myWebAPICaller = new WebAPICaller();
            myWebAPICaller.JsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(SelectedListModel[]));
            myWebAPICaller.ActionOnReadCompleted = new DlgtActionOnReadCompleted(ActionOnReadCompleted_AutoSel);
            myWebAPICaller.GET("parkingmaster/GetSearchParkingMaster/", "Latitude=" + Latitude + "&Longitude=" + Longitude);
            //http://localhost:62073/api/parkingmaster/GetSearchParkingMaster?Latitude=1&Longitude=1
        }
        void ActionOnReadCompleted_AutoSel(object myList)
        {
            SelectedListModel selItem;
            SelectedListModel tempItem;
            if (myList == null) return;
            SelectedListModel[] EntityList;
            EntityList = (SelectedListModel[])myList;
            if (EntityList.Length <= 0)
                return;
            selItem = EntityList[0];
            for (int i = 0; i < lstParking.Items.Count; i++)
            {
                tempItem = (SelectedListModel)lstParking.Items[i];
                if (tempItem.ListText == selItem.ListText)
                {
                    lstParking.SelectedIndex = i;
                    return;
                }
            }
        }
        private bool SelectParking_ByGeoLoc(string locAddress)
        {
            SelectedListModel item;
            for (int i = 0; i < lstParking.Items.Count; i++)
            {
                item = (SelectedListModel)lstParking.Items[i];
                if (locAddress.Contains(item.ListText .ToString()))
                {
                    lstParking.SelectedIndex = i;
                    return true;
                }
            }
            return false;
        }
        private void GoForPark(ParkingMode ParkMode)
        {
            SmartParking.API.Models.SelectedListModel selItem = (SmartParking.API.Models.SelectedListModel)lstParking.Items[lstParking.SelectedIndex];
            IsolatedStorageSettings.ApplicationSettings[_SelParking] = selItem.ListValue;
            ParkingBase.NavigateToParking(NavigationService, ParkMode);
        }
        private void imgContinue_Tap()
        {
            GoForPark(ParkingMode.Regular);
        }
        private void imgAdvPark_Tap()
        {
            GoForPark(ParkingMode.AdvancePark );
        }
        private void imgUsrProf_Tap()
        {
            NavigationService.Navigate(new Uri("/UserProfile.xaml", UriKind.Relative));
        }
        private void imgWallet_Tap()
        {
            NavigationService.Navigate(new Uri("/WalletTrans.xaml", UriKind.Relative));
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadParkingList();
        }
        private void lstParking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstParking.SelectedIndex == -1) return;
        }
        private async void SelectParking_ByGeoLoc()
        {
            Geolocator geolocator = new Geolocator();
            string Latitude;
            string Longitude;
            geolocator.DesiredAccuracyInMeters = 50;
            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));
                Latitude = geoposition.Coordinate.Latitude.ToString();
                Longitude = geoposition.Coordinate.Longitude.ToString();
                //GetCurrentLocation(Latitude, Longitude);
                SelectParkingAuto(Latitude, Longitude);
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    MessageBox.Show("location  is disabled in phone settings.");
                }
            }
        }
        //private void GetCurrentLocation(string Latitude, string Longitude)
        //{
        //    WebAPICaller myWebAPICaller = new WebAPICaller();
        //    myWebAPICaller.JsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(class3));
        //    myWebAPICaller.ActionOnReadCompleted = new DlgtActionOnReadCompleted(ActionOn_GetCurrentLocation);
        //    string myUri = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key=AIzaSyC57pgWFTS_XCAOQkRJb4UEgTmAxAbGlow";
        //    myUri = string.Format(myUri, Latitude, Longitude);
        //    myWebAPICaller.GET_Custom(myUri );
        //}
        void ActionOn_GetCurrentLocation(object location)
        {
            try
            {
                class3 myLoc = (class3)location;
                if (myLoc != null)
                    if(myLoc.results.Length > 0)
                    {
                        //MessageBox.Show("Your Location is: " + myLoc.results[0].formatted_address);
                        SelectParking_ByGeoLoc(myLoc.results[0].formatted_address);
                    }
            }
            catch (Exception e) { }
        }

       
    }
    public class class3
    {
       public  class2[] results;
    }
    public class class2
    {
        public class1[] address_components;
        public string formatted_address;
    }
    public class class1
    {
        public string long_name;
        public string short_name;
        public string[] types;
    }
}