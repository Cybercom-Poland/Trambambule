using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule.Achievements.Helpers
{
    public class AchievementTextWithIds
    {
        public string Text { get; set; }
        public int AchievementId { get; set; }
        public int PlayerId { get; set; }

        public AchievementTextWithIds(string text, int achievementId, int playerId)
        {
            Text = text;
            AchievementId = achievementId;
            PlayerId = playerId;
        }
    }
}