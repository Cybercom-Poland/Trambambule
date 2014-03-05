using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Trambambule.Achievements.Helpers;

namespace Trambambule.Achievements
{
    public class AchievementsService
    {
        private const string CalculationMethodName = "CalculateAchievement";
        private const string InheritedInterfaceName = "Trambambule.Achievements.IAchievement";

        List<Player> players;
        public AchievementsService(List<Player> _players)
        {
            players = _players;
        }

        public List<AchievementTextWithIds> RecalculateAchievements(bool firstTime)
        {
            List<AchievementTextWithIds> achievementsData = new List<AchievementTextWithIds>();

            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<Achievement> achievements = context.Achievements.ToList();
                foreach (Achievement achievement in achievements)
                {
                    foreach (Player player in players)
                    {
                        string result = CalculateMethod(player, achievement, context, firstTime);
                        if (result != "")
                            achievementsData.Add(new AchievementTextWithIds(result, achievement.Id, player.Id));
                    }
                }

                context.SubmitChanges();
            }

            return achievementsData;
        }

        private static string CalculateMethod(Player player, Achievement achievement, TrambambuleDBContextDataContext context, bool firstTime)
        {
            string result = "";
            Type t = Type.GetType(achievement.Class);
            if (t != null)
            {
                if (t.IsSubclassOf(typeof(AchievementBase)))
                {
                    var constructor = t.GetConstructor(new Type[] { });
                    if (constructor != null)
                    {
                        object o = constructor.Invoke(null);
                        if (o != null)
                        {
                            MethodInfo mi = t.GetMethod(CalculationMethodName);
                            if (mi != null)
                                result = (mi.Invoke(o, new object[] { firstTime, player, achievement, context })).ToString();
                        }
                    }
                }
            }
            return result;
        }
    }
}