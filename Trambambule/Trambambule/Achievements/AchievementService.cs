using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule.Achievements
{
    public class AchievementService
    {
        private IList<Player> _players;
        private IList<IAchievement> _achievements;
        private TrambambuleDBContextDataContext _context;

        public AchievementService(IList<Player> players, TrambambuleDBContextDataContext context)
        {
            _context = context;
            _players = players;
            _achievements = new List<IAchievement>()
            {
                new BestWeekWin(_context),
            };
        }

        public void Recalculate()
        {
            foreach (var achievemnt in _achievements)
            {
                var players = achievemnt.Calculate();
                if (players != null && players.Any())
                {
                    foreach (var player in players)
                    {
                        player.Achievements.Add(achievemnt);
                    }
                }
            }
        }
    }
}