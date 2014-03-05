using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule.Stats.Helpers
{
    public class BestStat
    {
        public Player Player { get; set; }
        public int Result { get; set; }
        public string Comment { get; set; }

        public BestStat(Player player, int result, string comment)
        {
            Player = player;
            Result = result;
            Comment = comment;
        }
    }
}