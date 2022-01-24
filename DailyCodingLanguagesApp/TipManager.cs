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

        // Send this event when the tips dictionary has changed
        public event EventHandler TipsChanged;

        /// <summary>
        /// Invokes TipsChangedEvent and calls SetLatestTipDate
        /// </summary>
        private void OnTipsChanged()
        {
            SetLatestTipDate();
            TipsChanged?.Invoke(this, EventArgs.Empty);

        }
        public async Task Start()
        {
            tips = FileBackupIO.LoadTips();
            if (tips.Count != 0)
            {
                OnTipsChanged();
            }
            else
            {
                // Load embedded tips, set the date to the latest, update the page.
                tips = EmbeddedFileIO.LoadTips();
                OnTipsChanged();
            }

            // Load github tips, set the date to the latest, update the page.
            tips = await GitHubFileIO.LoadTips();
            FileBackupIO.SaveTips(tips);
            OnTipsChanged();
        }

        public async Task UpdateTips()
        {
            tips = await GitHubFileIO.LoadTips();
            FileBackupIO.SaveTips(tips);
            OnTipsChanged();
        }

        /// <summary>
        /// Either set currentData to be todays tip, or the latest tip
        /// </summary>
        private void SetLatestTipDate()
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

        /// <summary>
        /// Called when the user moves to a different tip
        /// </summary>
        /// <param name="dir">Direction</param>
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
