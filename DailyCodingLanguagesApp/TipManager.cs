using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyCodingLanguagesApp
{
    public class TipManager
    {
        private SortedDictionary<DateTime, LanguageOfTheDay> tips = new SortedDictionary<DateTime, LanguageOfTheDay>();
        private DateTime currentDate = DateTime.Now;
        public TipPage tipPage = null;

        public async Task Start()
        {
            // Load embedded tips, set the date to the latest, update the page.
            tips = EmbeddedFileIO.LoadTips();
            SetLatestTipDate();
            tipPage.UpdateCurrentTip();

            // Load github tips, set the date to the latest, update the page.
            tips = await GitHubFileIO.LoadTips();
            SetLatestTipDate();
            tipPage.UpdateCurrentTip();
            
        }

        public async Task UpdateTips()
        {
            tips = await GitHubFileIO.LoadTips();
            SetLatestTipDate();
            tipPage.UpdateCurrentTip();
        }

        /// <summary>
        /// Either set currentData to be todays tip, or the latest tip
        /// </summary>
        public void SetLatestTipDate()
        {
            currentDate = DateTime.Now.Date;
            if (!tips.ContainsKey(currentDate))
            {
                if (tips.Count() != 0)
                {
                    currentDate = tips.Keys.Last();
                }
            }
        }

        /// <summary>
        /// Get the current tip if the date is a valid key
        /// </summary>
        /// <returns></returns>
        public LanguageOfTheDay GetCurrentTip()
        {
            if (!tips.ContainsKey(currentDate))
            {
                return null;
            }
            return tips[currentDate];
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
    }
}
