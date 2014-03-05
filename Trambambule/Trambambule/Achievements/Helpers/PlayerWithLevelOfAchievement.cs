using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule.Achievements.Helpers
{
    public class PlayerWithLevelOfAchievement
    {
        public Player Player { get; set; }
        public int LevelOfAchievement { get; set; }
        public string ToNextLevelLabel { get; set; }
        
        public PlayerWithLevelOfAchievement(Player player, int levelOfAchievement, string toNextLevelLabel)
        {
            Player = player;
            LevelOfAchievement = levelOfAchievement;
            ToNextLevelLabel = toNextLevelLabel;
        }
    }

    public class PlayerWithAchievement
    {
        public Player Player { get; set; }
        public int LevelOfAchievement { get; set; }
        public string ToNextLevelLabel { get; set; }
        public Achievement Achievement { get; set; }

        public PlayerWithAchievement(Player player, int levelOfAchievement, string toNextLevelLabel, Achievement achievement)
        {
            Player = player;
            LevelOfAchievement = levelOfAchievement;
            ToNextLevelLabel = toNextLevelLabel;
            Achievement = achievement;
        }
    }

    public class PlayersAchievements
    {
        public Player Player { get; set; }
        public List<AchievementWithCurrentLevel> Achievements { get; set; }

        public PlayersAchievements()
        {

        }

        public PlayersAchievements(Player player, List<AchievementWithCurrentLevel> achievements)
        {
            Player = player;
            Achievements = achievements;
        }
    }

    public class AchievementWithCurrentLevel
    {
        public Achievement Achievement { get; set; }
        public int Level { get; set; }

        public AchievementWithCurrentLevel(Achievement achievement, int level)
        {
            Achievement = achievement;
            Level = level;
        }
    }
}
