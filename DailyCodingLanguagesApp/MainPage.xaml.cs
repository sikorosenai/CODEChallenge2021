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
            UpdateCurrentTip();
        }
       
        void UpdateCurrentTip()
        {
            var tips = (App.Current as DailyCodingLanguagesApp.App).GetCurrentTip();
            if (tips != null)
            {
                Language.Text = tips.Language;
                Info.Text = tips.Info;
                Question.Text = tips.Question;
                Date.Text = tips.Date.ToShortDateString();

            }
            // Error message will be displayed if the loop is not executed
        }
        void OnEntryCompleted(object sender, EventArgs e)
        {
            string input = ((Entry)sender).Text;
          

            var tips = (App.Current as DailyCodingLanguagesApp.App).GetCurrentTip();
            if (tips != null)
            {
                var answer = tips.Answer;
                if (input == answer)
                {
                    Answer.Text = "Correct!";
                    Answer.TextColor = Color.Green;
                }
                else
                {
                    Answer.Text = "Wrong :(";
                    Answer.TextColor = Color.Red;
                }
            }
            
        }
        private void BButton_Clicked(object sender, EventArgs e)
        {
            (App.Current as DailyCodingLanguagesApp.App).ChangeTip(1);
            UpdateCurrentTip();
        }

        private void FButton_Clicked(object sender, EventArgs e)
        {
            (App.Current as DailyCodingLanguagesApp.App).ChangeTip(-1);
            UpdateCurrentTip();
        }


    }
}
