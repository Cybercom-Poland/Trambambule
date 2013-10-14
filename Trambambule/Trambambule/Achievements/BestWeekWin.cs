using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule.Achievements
{
    public class BestWeekWin : IAchievement
    {
        private TrambambuleDBContextDataContext _context;
        public BestWeekWin(TrambambuleDBContextDataContext context)
        {
            _context = context;
        }

        public string Label
        {
            get { return "Najlepszy wynik tygodnia"; }
        }

        public IEnumerable<Player> Calculate()
        {
            var query = from p in _context.TeamMatchPlayers
                        where p.Timestamp >= DateTime.Now.StartOfWeek(DayOfWeek.Monday)
                        orderby p.RatingChange descending
                        select p.Player;
            return new[] { query.FirstOrDefault() };
        }
    }
}