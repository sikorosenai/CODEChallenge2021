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
    public partial class MainPage : ContentPage
    { 
        public MainPage()
        {
            InitializeComponent();
            // Derive app instead of base application

            Connectivity.ConnectivityChanged += ConnectivityChangedHandler;
            UpdateCurrentTip();
        }

        private void ConnectivityChangedHandler(object sender, ConnectivityChangedEventArgs e)
        {
            string networkStatus = e.NetworkAccess.ToString();
            if (networkStatus == "None")
            {
                Navigation.PushAsync(new ConnectivityPage());
            }        
        }

        void UpdateCurrentTip()
        {
            var tips = (App.Current as DailyCodingLanguagesApp.App).GetCurrentTip();

            // Error message will be displayed if the following if statement is not executed
            if (tips != null)
            {
                Language.Text = tips.Language;
                Info.Text = tips.Info;
                Question.Text = tips.Question;
                Date.Text = tips.Date.ToShortDateString();
                Answer.Text = "";
                UserInput.Text = "";
            }
            
            var status = (App.Current as DailyCodingLanguagesApp.App).TipChangePossible();  
            Back.IsEnabled = status.backwardPos;
            Forward.IsEnabled = status.forwardPos;
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

        private void Notification_Clicked(object sender, EventArgs e)
        {
            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "Test",
                Description = "Test Description",
                ReturningData = "Dummy data", // Returning data when tapped on notification.
                Repeats = NotificationRepeat.Daily,
                NotifyTime = DateTime.Now.AddSeconds(5)
            };


        }

    }
}
