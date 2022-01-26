using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.LocalNotification;
using Xamarin.CommunityToolkit.UI.Views;

namespace DailyCodingLanguagesApp
{
    //research why here
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectivityPage : Popup
    {
        public ConnectivityPage()
        {
            InitializeComponent();
            //Connectivity.ConnectivityChanged += ConnectivityChangedHandler;
        }

        private void CloseBtn_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        /// <summary>
        /// Connection status has changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectivityChangedHandler(object sender, ConnectivityChangedEventArgs e)
        {
            string networkStatus = e.NetworkAccess.ToString();
            if (networkStatus != "None")
            {
                //fix later
                //Navigation.PushAsync(new TipPage());
            }
        }
    }
}