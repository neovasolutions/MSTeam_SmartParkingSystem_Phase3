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
    public partial class UCParkCommand : UserControl
    {
        public ActionOnImgTap Home_Tap { set; get; }
        public ActionOnImgTap Park_Tap { set; get; }
        public ActionOnImgTap UnPark_Tap { set; get; }
        public ActionOnImgTap UserProf_Tap { set; get; }

        public ActionOnImgTap AdvPark_Tap { set; get; }
        public ActionOnImgTap Wallet_Tap { set; get; }
        public UCParkCommand()
        {
            InitializeComponent();
        }
        public void SetMeInAdvanceParkMode()
        {
            HideAllCommands();
            imgHome.Visibility = System.Windows.Visibility.Visible;
        }
        public void SetMeInRegularParkMode()
        {
            HideAllCommands();
            imgHome.Visibility = System.Windows.Visibility.Visible;
            imgPark.Visibility = System.Windows.Visibility.Visible;
            imgUnPark.Visibility = System.Windows.Visibility.Visible;
        }
        public void SetMeInUserProfileMode()
        {
            HideAllCommands();
            imgHome.Visibility = System.Windows.Visibility.Visible;
        }
        public void SetMeInWalletMode()
        {
            HideAllCommands();
            imgHome.Visibility = System.Windows.Visibility.Visible;
        }
        public void SetMeInParkSelMode()
        {
            HideAllCommands();
            imgAdvPark.Visibility = System.Windows.Visibility.Visible;
            imgPark.Visibility = System.Windows.Visibility.Visible;
            imgWallet.Visibility = System.Windows.Visibility.Visible ;
            imgUsrProf.Visibility = System.Windows.Visibility.Visible;
        }
        private void HideAllCommands()
        {
            imgHome.Visibility = System.Windows.Visibility.Collapsed;
            imgAdvPark.Visibility = System.Windows.Visibility.Collapsed;
            imgPark.Visibility = System.Windows.Visibility.Collapsed ;
            imgUnPark.Visibility = System.Windows.Visibility.Collapsed;
            imgWallet.Visibility = System.Windows.Visibility.Collapsed;
            imgUsrProf.Visibility = System.Windows.Visibility.Collapsed;
        }
        public void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
        }
        public void imgHome_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Home_Tap();
        }
        public void imgPark_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Park_Tap();
        }
        public void imgUnPark_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UnPark_Tap();
        }
        public void imgUsrProf_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
            UserProf_Tap();
        }

        private void imgWallet_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Wallet_Tap();
        }
        private void imgAdvPark_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            AdvPark_Tap();
        }
    }
}
