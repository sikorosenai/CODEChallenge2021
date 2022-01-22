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

namespace DailyCodingLanguagesApp
{
    //research why here
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectivityPage : ContentPage
    {
        public ConnectivityPage()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += ConnectivityChangedHandler;
        }

        private void RetryBtn_Clicked(object sender, EventArgs e)
        {
            Connectivity.ConnectivityChanged += ConnectivityChangedHandler;
        }

        private void ConnectivityChangedHandler(object sender, ConnectivityChangedEventArgs e)
        {
            string networkStatus = e.NetworkAccess.ToString();
            if (networkStatus != "None")
            {
                Navigation.PushAsync(new TipPage());
            }
        }
    }
}