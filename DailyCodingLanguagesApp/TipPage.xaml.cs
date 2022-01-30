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
using Xamarin.CommunityToolkit.Extensions;
using System.Text.RegularExpressions;

namespace DailyCodingLanguagesApp
{
    public partial class TipPage : ContentPage
    {
        private TipManager tipManager = null;

        public TipPage(TipManager theManager)
        {
            // Passed theManager is equal to previous tipManager made, so that its functions can be used
            tipManager = theManager;
            InitializeComponent();
            // Derive app instead of base application

            Connectivity.ConnectivityChanged += ConnectivityChangedHandler;
        }

        private void ConnectivityChangedHandler(object sender, ConnectivityChangedEventArgs e)
        {
            string networkStatus = e.NetworkAccess.ToString();
            if (networkStatus == "None")
            {
                Navigation.ShowPopup(new ConnectivityPage());
            }        
        }

        /// <summary>
        /// Displays tip in Xamarin page
        /// </summary>
        public void UpdateCurrentTip()
        {
            var tip = tipManager.GetCurrentTip();

            // Error message will be displayed if the following if statement is not executed
            if (tip != null)
            {
                Language.Text = tip.Language;
                Info.Text = tip.Info;
                Question.Text = tip.Question;
                Date.Text = tip.Date.ToShortDateString();
                Answer.Text = "";
                UserInput.Text = "";
            }
            
            var status = tipManager.TipChangePossible();  
            Back.IsEnabled = status.backwardPos;
            Forward.IsEnabled = status.forwardPos;
        }
        void OnEntryCompleted(object sender, EventArgs e)
        {
            string input = ((Entry)sender).Text;
            var tip = tipManager.GetCurrentTip();
            if (tip != null)
            {
                var answer = tip.Answer;
                var conAnswer = Regex.Replace(answer, "[\r]", string.Empty);
                if (input == conAnswer)
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

        /// <summary>
        /// When Back button is clicked, go to the previous tip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BButton_Clicked(object sender, EventArgs e)
        {
            tipManager.ChangeTip(-1);
            UpdateCurrentTip();
        }

        private void FButton_Clicked(object sender, EventArgs e)
        {
            tipManager.ChangeTip(1);
            UpdateCurrentTip();
        }
    }
}
