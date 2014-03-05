using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trambambule.Achievements.Helpers;

namespace Trambambule
{
    public class DataAccess
    {
        public const string PlayersNamesAndSurnamesWithoutPolishCharsCacheName = "playersWithoutPolishChars";
        public const string PlayersListCacheName = "playersList";
        public const string PlayersAchievementsCacheName = "playersAchievementsList";
        public const string AllAchievementsCacheName = "AllAchievementsList";
        public const string OverallStatsCacheName = "OverallStats";
        public const int CacheTimeInSeconds = 3600;

        public static List<Player> GetPlayers()
        {
            List<Player> playersList = new List<Player>();
            if (HttpContext.Current.Cache[PlayersListCacheName] == null)
            {
                using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
                {
                    playersList = context.Players.ToList();                    
                }
                HttpContext.Current.Cache.Insert(DataAccess.PlayersListCacheName, playersList, null, DateTime.Now.AddSeconds(CacheTimeInSeconds), TimeSpan.Zero);
            }
            else
            {
                playersList = (List<Player>)HttpContext.Current.Cache[PlayersListCacheName];
            }
            return playersList;
        }

        public static Player GetPlayer(string fullName)
        {
            return GetPlayers().FirstOrDefault(p => p.FirstName + " " + p.LastName == fullName);
        }

        public static Player GetPlayer(int id)
        {
            return GetPlayers().FirstOrDefault(p => p.Id == id);
        }

        public static List<Trambambule.PlayerHelper.PlayersNameAndLastNameWithLocation> GetPlayersNamesAndSurnamesWithoutPolishChars()
        {
            List<Trambambule.PlayerHelper.PlayersNameAndLastNameWithLocation> playersNamesAndSurnames = new List<Trambambule.PlayerHelper.PlayersNameAndLastNameWithLocation>();
            if (HttpContext.Current.Cache[PlayersNamesAndSurnamesWithoutPolishCharsCacheName] == null)
            {
                List<Player> players = GetPlayers();
                foreach (Player p in players)
                {
                    string nameAndSurname = (Common.CreateTextCompareString(p.FirstName) + " " + Common.CreateTextCompareString(p.LastName));

                    playersNamesAndSurnames.Add(new Trambambule.PlayerHelper.PlayersNameAndLastNameWithLocation(nameAndSurname, p.Location));
                }

                HttpContext.Current.Cache.Insert(DataAccess.PlayersNamesAndSurnamesWithoutPolishCharsCacheName, playersNamesAndSurnames, null, DateTime.Now.AddSeconds(CacheTimeInSeconds), TimeSpan.Zero);
            }
            else
            {
                playersNamesAndSurnames = (List<Trambambule.PlayerHelper.PlayersNameAndLastNameWithLocation>)HttpContext.Current.Cache[PlayersNamesAndSurnamesWithoutPolishCharsCacheName];
            }

            return playersNamesAndSurnames;
        }

        public static List<PlayersAchievements> GetPlayersWithAchievements()
        {
            List<PlayersAchievements> players = new List<PlayersAchievements>();
            if (HttpContext.Current.Cache[DataAccess.PlayersAchievementsCacheName] == null)
            {
                using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
                {
                    foreach (Player p in DataAccess.GetPlayers())
                    {
                        var achievements = (from ac in context.AchievementPlayer
                                            join a in context.Achievements
                                            on ac.AchievementId equals a.Id
                                            where ac.PlayerId == p.Id && ac.LevelOfAchievement >= 1
                                            
                                            select new AchievementWithCurrentLevel(a, ac.LevelOfAchievement)).ToList();

                        PlayersAchievements pa = new PlayersAchievements(p, achievements);
                        players.Add(pa);
                    }
                }

                HttpContext.Current.Cache.Insert(DataAccess.PlayersAchievementsCacheName, players, null, DateTime.Now.AddSeconds(DataAccess.CacheTimeInSeconds), TimeSpan.Zero);
            }
            else
            {
                players = (List<PlayersAchievements>)HttpContext.Current.Cache[DataAccess.PlayersAchievementsCacheName];
            }

            return players;
        }

    }
}

