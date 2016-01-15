using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SmartParking.Models;
using System.IO.IsolatedStorage;
using System.Windows.Data;

namespace SmartParking
{
    public partial class WalletTrans : PhoneApplicationPage
    {
        public WalletTrans()
        {
            InitializeComponent();
            ParkCommand.SetMeInWalletMode();
            ParkCommand.Home_Tap = new ActionOnImgTap(imgHome_Tap);
        }
        private void LoadWalletTrans()
        {
            WebAPICaller myWebAPICaller = new WebAPICaller();
            myWebAPICaller.JsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(WalletTransModel[]));
            myWebAPICaller.ActionOnReadCompleted = new DlgtActionOnReadCompleted(ActionOnReadCompleted);
            myWebAPICaller.GET("WalletTrans/GetWalletTrans?userID=" + ParkingBase.UserID);
        }
        void ActionOnReadCompleted(object myList)
        {
            if (myList == null) return;
            WalletTransModel[] EntityList;
            EntityList = (WalletTransModel[])myList;
            //List<WalletTransModel> lst = new List<WalletTransModel>(myList);

            IList<WalletTransModel> lstSrc = EntityList;

            lstWalletTrans.ItemsSource = lstSrc;// new CollectionViewSource { Source = (CollectionViewSource)lstSrc };

            WebAPICaller myWebAPICaller = new WebAPICaller();
            myWebAPICaller.ActionOnReadCompleted = new DlgtActionOnReadCompleted(ActionOnReadCompleted_Amount);
            myWebAPICaller.JsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(int));
            myWebAPICaller.GET("UserProfile/GetWalletAmount?userID=" + ParkingBase.UserID);
        }
        void ActionOnReadCompleted_Amount(object amount)
        {
            if (amount == null) return;
            txtAmount.Text = amount.ToString();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWalletTrans();
        }
        private void imgHome_Tap()
        {
            NavigationService.Navigate(new Uri("/ParkingSel.xaml", UriKind.Relative));
        }
    }
}