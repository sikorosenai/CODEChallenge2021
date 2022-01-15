using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace DailyCodingLanguagesApp
{

    public struct LanguageOfTheDay
    {
        public string Language;

        public string Info;

        public string Question;

        public string Answer;
    };

    public partial class App : Application
    {
        private List<LanguageOfTheDay> tips = new List<LanguageOfTheDay>();

        public LanguageOfTheDay GetCurrentTip()
        {
            return tips.Last();
        }

        public App()
        {
            InitializeComponent();
            LoadTips();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
           
        }

        private void LoadTips()
        {
           

            // Read

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;

            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);

                // Grab asset
                System.IO.Stream s = assembly.GetManifestResourceStream("DailyCodingLanguagesApp.Assets._2022.January.14th.txt");
                System.IO.StreamReader sr = new System.IO.StreamReader(s);

                // Split              
                var lines = sr.ReadToEnd();
                sr.Close();
                string[] l = lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                if (l.Length == 4)
                {
                    LanguageOfTheDay tip;
                    tip.Language = l[0];
                    tip.Info = l[1];
                    tip.Question = l[2];
                    tip.Answer = l[3];

                    tips.Add(tip);
                }


            }
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


    }
}
