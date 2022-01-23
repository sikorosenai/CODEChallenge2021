using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace DailyCodingLanguagesApp
{
    class EmbeddedFileIO  
    { 
        public static SortedDictionary<DateTime, LanguageOfTheDay> LoadTips()
        {
            var tips = new SortedDictionary<DateTime, LanguageOfTheDay>();
            // Read
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TipPage)).Assembly;

            foreach (var assets in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + assets);

                try
                {              
                    // Grab asset
                    System.IO.Stream s = assembly.GetManifestResourceStream(assets);
                    System.IO.StreamReader sr = new System.IO.StreamReader(s);

                    // date as key first then as parameter
                    var date = GetDateFromAssetPath(assets);
                    tips.Add(date, LanguageOfTheDay.Parse(date, sr.ReadToEnd()));
                    sr.Close();
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, String.Format("Invalid Tip File: {0}:{1}", assets, ex.Message));
                }
            }
            return tips;
        }

        private static DateTime GetDateFromAssetPath(string path)
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
            catch (Exception ex)
            {
                Debug.Assert(false, string.Format("Couldnt parse the date: {0}: {1}", path, ex.Message));
                throw ex;
            }
        }
    }
}
