using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace DailyCodingLanguagesApp
{
    public partial class App : Application
    {
        
        private TipPage tipPage;
        private TipManager tipManager = new TipManager();
        protected override async void OnStart()
        {
            // Add onStart to functions that want to know when TipManagerSaysTipsHaveChanged
            tipManager.TipsChanged += TipManagerSaysTipsHaveChanged;
            await tipManager.Start();
        }

        private void TipManagerSaysTipsHaveChanged(object sender, EventArgs e)
        {
            tipPage.UpdateCurrentTip();
        }

        public App()
        {
            InitializeComponent();
            
            //added for navigation
            tipPage = new TipPage(tipManager);
            MainPage = new NavigationPage(tipPage);
        }
        
        protected override void OnSleep()
        {
        }

        protected override async void OnResume()
        {
            await tipManager.UpdateTips();
        }
    }
}
