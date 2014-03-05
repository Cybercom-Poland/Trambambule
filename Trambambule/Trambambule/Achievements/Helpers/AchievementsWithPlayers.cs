using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule.Achievements.Helpers
{
    public class AchievementsWithPlayers
    {
        public Achievement Achievement { get; set; }
        public List<PlayerWithLevelOfAchievement> Players { get; set; }

        public AchievementsWithPlayers(Achievement achievement, List<PlayerWithLevelOfAchievement> players)
        {
            Achievement = achievement;
            Players = players;
        }
    }
}