using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule
{
    public class PlayerHelper
    {
        public static string GetPlayerName(Player player)
        {
            return player.FirstName + " " + player.LastName;
        }

        public static bool FillPlayerRating(ref TeamMatchPlayer currentMatchData, TeamMatchPlayer lastMatchData,
            Common.EResult result, int goalsScored, int goalsConcerned)
        {
            return true;
        }
    }
}