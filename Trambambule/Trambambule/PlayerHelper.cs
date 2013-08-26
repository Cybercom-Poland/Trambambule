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

        public static bool FillPlayersRating(
            ref TeamMatchPlayer playerA1MatchData,
            ref TeamMatchPlayer playerA2MatchData,
            ref TeamMatchPlayer playerB1MatchData,
            ref TeamMatchPlayer playerB2MatchData,
            TeamMatchPlayer playerA1LastMatchData,
            TeamMatchPlayer playerA2LastMatchData,
            TeamMatchPlayer playerB1LastMatchData,
            TeamMatchPlayer playerB2LastMatchData,
            int goalsA, int goalsB)
        {
            
            return true;
        }
    }
}