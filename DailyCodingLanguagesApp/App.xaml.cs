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
    public class LanguageOfTheDay
    {
        public static LanguageOfTheDay Parse(DateTime date, string text)
        {
            string[] l = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (l.Length == 4)
            {
                LanguageOfTheDay tip = new LanguageOfTheDay();
                tip.Language = l[0];
                tip.Info = l[1];
                tip.Question = l[2];
                tip.Answer = l[3];
                tip.Date = date;

                return tip;
            }
            return null;
        }

        public string Language;

        public string Info;

        public string Question;

        public string Answer;

        public DateTime Date;
    };

    public partial class App : Application
    {
        private SortedDictionary<DateTime, LanguageOfTheDay> tips = new SortedDictionary<DateTime, LanguageOfTheDay>();
        private DateTime currentDate = DateTime.Now;
        private TipPage tipPage;
        protected override async void OnStart()
        {
            tips = await FindTips();
            currentDate = DateTime.Now.Date;
            tipPage.UpdateCurrentTip();
        }

        // Called from TipPage.xaml.cs
        public LanguageOfTheDay GetCurrentTip()
        {
            if (!tips.ContainsKey(currentDate))
            {
                if (tips.Count() == 0)
                {
                    // If there are not tips for today or any other day
                    return null;
                }
                currentDate = tips.Keys.Last();
            }
            return tips[currentDate];
        }

        public App()
        {
            InitializeComponent();
            LoadTips();

            //added for navigation
            tipPage = new TipPage();
            MainPage = new NavigationPage(tipPage);
        }
        
        private DateTime GetDateFromAssetPath(string path)
        {
           
            try
            {
                var dateString = path.Split('.');
                if (dateString.Length < 5)
                {
                    Debug.Assert(false, "Date string not valid!");
                    throw new ArgumentOutOfRangeException(path);
                }

                var year = dateString[2].Trim('_');
                var month = dateString[3];
                var day = dateString[4];
                return DateTime.Parse(string.Format("{0}/{1}/{2}", year, month, day));
            }
            catch(Exception ex)
            {
                Debug.Assert(false, string.Format("Couldnt parse the date: {0}: {1}", path, ex.Message));
                throw ex;
            }
        }
        private void LoadTips()
        {
            // Read
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TipPage)).Assembly;

            foreach (var assets in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + assets);

                try
                {
                    var date = GetDateFromAssetPath(assets);            
                    // Grab asset
                    System.IO.Stream s = assembly.GetManifestResourceStream(assets);
                    System.IO.StreamReader sr = new System.IO.StreamReader(s);

                    // date as key first then as parameter
                    tips.Add(date, LanguageOfTheDay.Parse(date, sr.ReadToEnd()));
                    sr.Close();               
                }
                catch(Exception ex)
                {
                    Debug.Assert(false, String.Format("Invalid Tip File: {0}:{1}", assets, ex.Message));
                }
            }
        }

        //stackoverflow.com/a/18297009
        public void ChangeTip(int dir)
        { // param "dir" can be 1 or -1 to move index forward or backward
            List<DateTime> keys = new List<DateTime>(tips.Keys);
            int newIndex = keys.IndexOf(currentDate) + dir;
            if (newIndex < 0)
            {
                newIndex = 0;
            }
            else if (newIndex > tips.Count - 1)
            {
                newIndex = (tips.Count - 1);
            }
            currentDate = keys[newIndex];
        }

        public struct TipChangeStatus
        {
            public bool forwardPos;
            public bool backwardPos;
        };

        public TipChangeStatus TipChangePossible()
        {
            TipChangeStatus status;
            status.forwardPos = false;
            status.backwardPos = false;
            
            List<DateTime> keys = new List<DateTime>(tips.Keys);
           
            if (keys.IndexOf(currentDate) > 0)
            {
                status.backwardPos = true;
            }
            
            if (keys.IndexOf(currentDate) < (tips.Count - 1))
            {
                status.forwardPos = true;       
            }
            return status;
        }

        enum FileSearchType
        {
            Directory,
            Files
        };

        static async Task<string> ReadFileFromGithub(string contentsUrl)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApplication", "1"));
            return await httpClient.GetStringAsync(contentsUrl);
        }

        //https://markheath.net/post/list-and-download-github-repo-cs
        static async Task<List<string>> FindFilePath(FileSearchType searchType, string contentsUrl)
        {
            var paths = new List<string>();

            //https://api.github.com/repos/sikorosenai/DailyLang/contents/2022?ref=main
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApplication", "1"));
            var contentsJson = await httpClient.GetStringAsync(contentsUrl);
            var contents = (JArray)JsonConvert.DeserializeObject(contentsJson);
            foreach (var file in contents)
            {
                var fileType = (string)file["type"];
                if (fileType == "dir" && searchType == FileSearchType.Directory)
                {
                    paths.Add((string)file["url"]);
                }
                else if (fileType == "file" && searchType == FileSearchType.Files)
                {
                    paths.Add((string)file["download_url"]);
                }
            }
            return paths;
        }
        static async Task<SortedDictionary<DateTime, LanguageOfTheDay>> FindTips()
        {
            var result = new SortedDictionary<DateTime, LanguageOfTheDay>();
            var repo = "sikorosenai/DailyLang";
            var contentsUrl = $"https://api.github.com/repos/{repo}/contents/";

            // Unwraps list for each year
            var years = await FindFilePath(FileSearchType.Directory, contentsUrl);
            foreach (var yearPath in years) 
            {
                var months = await FindFilePath(FileSearchType.Directory, yearPath);
                // For each month
                foreach (var monthPath in months)
                {
                    var days = await FindFilePath(FileSearchType.Files, monthPath);
                    // For each day
                    foreach (var dayPath in days)
                    {
                        // Example path: https://raw.githubusercontent.com/sikorosenai/DailyLang/main/2022/February/1.txt

                        try
                        {
                            //https://regex101.com/r/qO5cX9/1
                            var exp = new Regex(@"\d\d\d\d\/\w+\/\d+");
                            var dateString = exp.Match(dayPath);

                            var tipDate = DateTime.Parse(dateString.Value);
                            var tipText = await ReadFileFromGithub(dayPath);

                            var tip = LanguageOfTheDay.Parse(tipDate, tipText);

                            //unwrap task string
                            result.Add(tipDate, tip);
                        }
                        catch(Exception ex)
                        {
                            Debug.Assert(false, String.Format("Invalid Tip Parsing: {0}", ex.Message));
                        }
                    }
                }
            }
            // Returns list of strings of contents of text files to where FindTips was called 
            return result;
        }

         
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
