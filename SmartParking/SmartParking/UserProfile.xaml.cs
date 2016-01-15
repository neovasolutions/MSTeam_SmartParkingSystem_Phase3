using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.IO.IsolatedStorage;
using SmartParking.API.Models;
using Windows.UI.Popups;
using System.Windows.Threading;

namespace SmartParking
{
    public partial class UserProfile : PhoneApplicationPage
    {
        private string _OTPNo;
        private DispatcherTimer _dispatcherTimer;
        public UserProfile()
        {
            InitializeComponent();
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += _dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            ParkCommand.SetMeInUserProfileMode();
            ParkCommand.Home_Tap = new ActionOnImgTap(imgHome_Tap);
        }

        void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
            HideOTPInput(true);
            _dispatcherTimer.Stop();
        }
        private void LoadUserProfile()
        {
            if (ParkingBase.UserID  != null)
            {
                WebAPICaller myWebAPICaller = new WebAPICaller();
                myWebAPICaller.JsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(UserProfileModel));
                myWebAPICaller.ActionOnReadCompleted = new DlgtActionOnReadCompleted(ActionOn_LoadUserProfile);
                myWebAPICaller.GET("UserProfile/GetUserProfile/" + ParkingBase.UserID);
            }
            else
            {
                txtUserName.Text = "";
                txtAddress.Text = "";
                txtMobileNo.Text = "";
                txtVehclNo.Text = "";
                txtCity.Text = "";
                txtPincode.Text = "";
                txtEmailID.Text = "";
                return;
            }
        }
        private void GoForOTP()
        {
            WebAPICaller myWebAPICaller = new WebAPICaller();
            myWebAPICaller.JsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
            myWebAPICaller.ActionOnReadCompleted = new DlgtActionOnReadCompleted(ActionOn_GetOTP);
            myWebAPICaller.GET("UserProfile/GetOTP?MobileNo=" + txtMobileNo.Text.Trim());
        }
        void ActionOn_GetOTP(object OTP)
        {
            _OTPNo = OTP.ToString();
            _dispatcherTimer.Start();
            MessageBox.Show("OTP: " + _OTPNo);
        }
        void ActionOn_LoadUserProfile(object usrProfl)
        {
            if (usrProfl == null) return;
            UserProfileModel model = (UserProfileModel)usrProfl;
            txtUserName.Text=model.FirstName ;
            txtAddress.Text=model.Address1 ;
            txtMobileNo.Text=model.MobileNumber ;
            txtVehclNo.Text=model.ActiveVehicleNumber ;
            txtCity.Text = model.City;
            txtPincode.Text = model.Pincode;
            txtEmailID.Text = model.EmailID; 
        }
        void ActionOn_SaveUserProfile(string userID)
        {
            if (userID  == null) return;
            IsolatedStorageSettings.ApplicationSettings[ParkingBase.C_UserID] = userID;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }
        void Save()
        {
            if (!IsValidInput())
            {
                MessageBox.Show("User Name is mandatory.");
                return;
            }
            WebAPICaller myWebAPICaller = new WebAPICaller();
            UserProfileModel model = new UserProfileModel();
            model.FirstName = txtUserName.Text;
            model.Address1 = txtAddress.Text;
            model.MobileNumber = txtMobileNo.Text;
            model.ActiveVehicleNumber = txtVehclNo.Text;
            model.City=txtCity.Text ;
            model.Pincode=txtPincode.Text ;
            model.EmailID=txtEmailID.Text ;
            if (ParkingBase.UserID != null)
            {
                model.UserID = System.Convert.ToInt32(ParkingBase.UserID);
                myWebAPICaller.POST("UserProfile/PostUpdateUserProfile/", model);
            }
            else
            {
                myWebAPICaller.ActionOnUploadCompleted = new DlgtActionOnUploadCompleted(ActionOn_SaveUserProfile);
                myWebAPICaller.POST("UserProfile/PostUserProfile/", model);
            }
            NavigationService.GoBack();
        }
        private bool IsValidInput()
        {
            if (txtUserName.Text == "")
                return false;
            else
                return true;
        }
        private void HideOTPInput(bool isHide)
        {
            if (!isHide)
            {
                OTPInput.Height = new GridLength(100);
                RowContinue.Height = new GridLength(0);
            }
            else
            {
                OTPInput.Height = new GridLength(0);
                RowContinue.Height = new GridLength(100);
            }
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserProfile();
        }
        private void imgBack_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            ParkingBase.NavigateToParking(NavigationService, ParkingMode.Regular );
        }
        private void imgSave_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            HideOTPInput(true);
            if (_OTPNo != txtOTP.Text)
            {
                MessageBox.Show("Entered OTP is not correct.");
                return;
            }
            else
                Save();
        }
        private void imgHome_Tap()
        {
            NavigationService.Navigate(new Uri("/ParkingSel.xaml", UriKind.Relative));
        }
        private void imgContinue_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            HideOTPInput(false);
            GoForOTP();
        }
    }
}