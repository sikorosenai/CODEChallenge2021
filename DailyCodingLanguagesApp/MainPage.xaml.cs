using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace DailyCodingLanguagesApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // Derive app instead of base application
            var tips = (App.Current as DailyCodingLanguagesApp.App).GetCurrentTip();
            Language.Text = tips.Language;
            Info.Text = tips.Info;
            Question.Text = tips.Question;
        }



        private void Button_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://github.com/sikorosenai");
        }





    }


}
