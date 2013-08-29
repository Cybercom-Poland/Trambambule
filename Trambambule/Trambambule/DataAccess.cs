using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule
{
    public class DataAccess
    {
        public static List<Player> GetPlayers()
        {
            if (HttpContext.Current.Cache["Players"] == null)
                using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
                {
                    HttpContext.Current.Cache.Add(
                        "Players", context.Players.ToList(),
                        null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                        new TimeSpan(1, 0, 0), System.Web.Caching.CacheItemPriority.Normal, null);
                }
            return (List<Player>)HttpContext.Current.Cache["Players"];
        }

        public static Player GetPlayer(string fullName)
        {
            return GetPlayers().FirstOrDefault(p => p.FirstName + " " + p.LastName == fullName);
        }

        public static Player GetPlayer(int id)
        {
            return GetPlayers().FirstOrDefault(p => p.Id == id);
        }
    }
}