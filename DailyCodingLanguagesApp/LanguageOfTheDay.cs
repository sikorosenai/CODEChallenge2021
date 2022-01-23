using System;
using System.Collections.Generic;
using System.Text;

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
}
