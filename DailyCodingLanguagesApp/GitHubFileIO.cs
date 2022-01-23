using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DailyCodingLanguagesApp
{
    class GitHubFileIO
    {
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
        public static async Task<SortedDictionary<DateTime, LanguageOfTheDay>> LoadTips()
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

                            DateTime tipDate = DateTime.Parse(dateString.Value);

                            var tipText = await ReadFileFromGithub(dayPath);

                            var tip = LanguageOfTheDay.Parse(tipDate, tipText);

                            //unwrap task string
                            result.Add(tipDate, tip);
                        }
                        catch (Exception ex)
                        {
                            Debug.Assert(false, String.Format("Invalid Tip Parsing: {0}", ex.Message));
                        }
                    }
                }
            }
            // Returns list of strings of contents of text files to where FindTips was called 
            return result;
        }
    }
}
