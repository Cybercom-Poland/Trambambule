using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule.Achievements
{
    public interface IAchievement
    {
        string CalculateAchievement(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context);

        bool CalculateLevel1(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string toNext);
        bool CalculateLevel2(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string toNext);
        bool CalculateLevel3(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string toNext);
    }
}
