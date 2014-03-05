using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule.Achievements.Helpers
{
    public abstract class AchievementBase : IAchievement
    {
        public string CalculateAchievement(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context)
        {
            int level = 0;
            string label = "";
            int newLevel = 0;
            var currentAchievement = context.AchievementPlayer.FirstOrDefault(w => w.AchievementId == achievement.Id && w.PlayerId == player.Id);
            if (currentAchievement != null)
                level = currentAchievement.LevelOfAchievement;
            else
                level = 0;

            string comment = "";
            switch (level)
            {
                case 0:
                    CaseLevel1(firstTime, player, achievement, context, ref label, ref newLevel, ref comment);
                    break;
                case 1:
                    CaseLevel2(firstTime, player, achievement, context, ref label, ref newLevel, ref comment);
                    break;
                case 2:
                    CaseLevel3(firstTime, player, achievement, context, ref label, ref newLevel, ref comment);
                    break;
            }

            //if (newLevel > level)
            //{
            if (currentAchievement == null)
            {
                currentAchievement = new AchievementPlayer();
                currentAchievement.LevelOfAchievement = newLevel;
                currentAchievement.AchievementId = achievement.Id;
                currentAchievement.ToNextLevelLabel = comment;
                currentAchievement.PlayerId = player.Id;

                context.AchievementPlayer.InsertOnSubmit(currentAchievement);
            }
            else
            {
                if (newLevel > level)
                {
                    currentAchievement.LevelOfAchievement = newLevel;
                }
                if (!string.IsNullOrEmpty(comment))
                {
                    currentAchievement.ToNextLevelLabel = comment;
                }
            }

            if (newLevel > level)
            {
                string levelName = Common.GetAchievementsLevelName(newLevel);
                return string.Format("Gracz {0} {1} uzyskał poziom {2} ({3}) w osiągnięciu \"{4}\".", player.FirstName, player.LastName, levelName, label, achievement.Title);
            }
            else
                return "";
            //}
            //else if (comment != "" && currentAchievement != null)
            //{
            //    currentAchievement.ToNextLevelLabel = comment;
            //}

            //return "";
        }

        private bool CaseLevel1(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, ref string label, ref int newLevel, ref string comment)
        {
            if (CalculateLevel1(firstTime, player, achievement, context, out comment))
            {
                string currentComment = comment;
                newLevel = 1;
                label = achievement.Level1Label;
                if (!CaseLevel2(firstTime, player, achievement, context, ref label, ref newLevel, ref comment))
                {
                    comment = currentComment;
                }
                return true;
            }
            else
                return false;
        }

        private bool CaseLevel2(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, ref string label, ref int newLevel, ref string comment)
        {
            if (CalculateLevel2(firstTime, player, achievement, context, out comment))
            {
                string currentComment = comment;
                newLevel = 2;
                label = achievement.Level2Label;
                if (!CaseLevel3(firstTime, player, achievement, context, ref label, ref newLevel, ref comment))
                {
                    comment = currentComment;
                }
                return true;
            }
            else
                return false;
        }

        private bool CaseLevel3(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, ref string label, ref int newLevel, ref string comment)
        {
            if (CalculateLevel3(firstTime, player, achievement, context, out comment))
            {
                newLevel = 3;
                label = achievement.Level3Label;
                //comment = "Osiągnięto najwyższy poziom w tym osiągnięciu";
                return true;
            }
            else
                return false;
        }

        public abstract bool CalculateLevel1(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string toNext);
        public abstract bool CalculateLevel2(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string toNext);
        public abstract bool CalculateLevel3(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string toNext);

    }
}