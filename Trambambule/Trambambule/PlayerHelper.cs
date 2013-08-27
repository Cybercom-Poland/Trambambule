using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule
{
    public class PlayerHelper
    {
        private static readonly long SECONDS_IN_YEAR = 31536000;
        private static readonly double INITIAL_RATING = 1500;
        private static readonly double MAX_RD = 350;
        private static readonly double STABLE_RD = 50;
        private static readonly double MAGIC_C_CONSTANT = Math.Sqrt((MAX_RD * MAX_RD - STABLE_RD * STABLE_RD) / SECONDS_IN_YEAR);

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
            if (playerA1LastMatchData == null)
            {
                playerA1LastMatchData = CreateEmptyMatchData();
            }
            if (playerA2LastMatchData == null)
            {
                playerA2LastMatchData = CreateEmptyMatchData();
            }
            if (playerB1LastMatchData == null)
            {
                playerB1LastMatchData = CreateEmptyMatchData();
            }
            if (playerB2LastMatchData == null)
            {
                playerB2LastMatchData = CreateEmptyMatchData();
            }
            playerA1LastMatchData.RD = CalculateCurrentRD(playerA1LastMatchData, playerA1MatchData.Timestamp);
            playerA2LastMatchData.RD = CalculateCurrentRD(playerA2LastMatchData, playerA2MatchData.Timestamp);
            playerB1LastMatchData.RD = CalculateCurrentRD(playerB1LastMatchData, playerB1MatchData.Timestamp);
            playerB2LastMatchData.RD = CalculateCurrentRD(playerB2LastMatchData, playerB2MatchData.Timestamp);
            FillSinglePlayerRating(ref playerA1MatchData, playerA1LastMatchData, playerA2LastMatchData, playerB1LastMatchData, playerB2LastMatchData, goalsA, goalsB);
            FillSinglePlayerRating(ref playerA2MatchData, playerA2LastMatchData, playerA1LastMatchData, playerB1LastMatchData, playerB2LastMatchData, goalsA, goalsB);
            FillSinglePlayerRating(ref playerB1MatchData, playerB1LastMatchData, playerB2LastMatchData, playerA1LastMatchData, playerA2LastMatchData, goalsB, goalsA);
            FillSinglePlayerRating(ref playerB2MatchData, playerB2LastMatchData, playerB1LastMatchData, playerA1LastMatchData, playerA2LastMatchData, goalsB, goalsA);
            return true;
        }

        private static TeamMatchPlayer CreateEmptyMatchData()
        {
            TeamMatchPlayer matchData = new TeamMatchPlayer();
            matchData.Rating = INITIAL_RATING;
            matchData.RD = MAX_RD;
            matchData.Timestamp = new DateTime(2012, 08, 23);
            return matchData;
        }

        private static void FillSinglePlayerRating(
            ref TeamMatchPlayer newMatchData,
            TeamMatchPlayer oldMatchData,
            TeamMatchPlayer allyMatchData,
            TeamMatchPlayer firstOppMatchData,
            TeamMatchPlayer secondOppMatchData,
            int ourGoals, int oppGoals)
        {
            double result = CalculateResult(ourGoals, oppGoals);
        }

        private static double CalculateResult(int ourGoals, int oppGoals)
        {
            if (ourGoals == 0 && oppGoals == 0)
            {
                return 0.5;
            }
            else
            {
                return ((double)ourGoals) / (ourGoals + oppGoals);
            }
        }

        public static double CalculateCurrentRD(TeamMatchPlayer lastMatchData, DateTime currentTimestamp)
        {
            double currentRD = (double) lastMatchData.RD;
            TimeSpan timeSpan = currentTimestamp - lastMatchData.Timestamp;
            double timeInSeconds = timeSpan.TotalSeconds;
            return Math.Min(MAX_RD, Math.Sqrt(currentRD * currentRD + MAGIC_C_CONSTANT * MAGIC_C_CONSTANT * timeInSeconds));
        }
    }
}