using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.GameStatisticsModule
{
    class OnlineGameStatistics : GameStatisticsModule
    {
        private const string USER_STATS_API = "updateUserApi.php";
        private const string GAME_API = "updateGameApi.php";
        private const string GAME_STATS_API = "updateGameStatisticsApi.php";
        private const string OVERALL_STATS_API = "OverallStatisticsApi.php";

        RestClientStats restClientStats = new RestClientStats();

        public override void UpdateUserStats(int _win, int _loss, string _usename)
        {
            JSONObject userStatsJson = restClientStats.CreateJsonForPlayerStats(_win, _loss, _usename);
            restClientStats.UpdateDB(userStatsJson, USER_STATS_API, ConnectionModule.ConnectionModule.HttpResquest.POST);
        }

        public override void UpdateGame(string _winningTeam, int _arenaId, ConnectionModule.ConnectionModule.HttpResquest httpResquestType)
        {
            JSONObject gameJson = restClientStats.CreateJsonForGame(_winningTeam, _arenaId);
            restClientStats.UpdateDB(gameJson, GAME_API, ConnectionModule.ConnectionModule.HttpResquest.POST);
        }

        public override void UpdateGameStats(Dictionary<string, int> _playerOverallStat, int _userId)
        {
            JSONObject gameStatsJson = restClientStats.CreateJsonForGameStats(_playerOverallStat, _userId);
            restClientStats.UpdateDB(gameStatsJson, GAME_STATS_API, ConnectionModule.ConnectionModule.HttpResquest.POST);
        }

        private int StringJsonToInt(string jsonString)
        {
            if (jsonString.Contains("null"))
            {
                return 0;
            }


            string stringToParse = "";
            for (int j = 0; j < jsonString.Length; j++)
            {
                int result;
                if (int.TryParse(jsonString[j].ToString(), out result))
                {
                    stringToParse += result;
                }
            }
            if (stringToParse == "") return 0;

            return int.Parse(stringToParse);
        }

        public override Dictionary<string, int> GetOverallStats(int _userId)
        {
            JSONObject overallStatsJson = restClientStats.CreateJsonForUserOverallStats(_userId);
            overallStatsJson = restClientStats.UpdateDB(overallStatsJson, OVERALL_STATS_API, ConnectionModule.ConnectionModule.HttpResquest.POST);

            Dictionary<string, int> playerOverallStat = new Dictionary<string, int>();

            playerOverallStat.Add("Assists", StringJsonToInt(overallStatsJson["assists"].ToString()));
            playerOverallStat.Add("Goals", StringJsonToInt(overallStatsJson["goals"].ToString()));
            playerOverallStat.Add("Deaths", StringJsonToInt(overallStatsJson["deaths"].ToString()));
            playerOverallStat.Add("Shots To Goal", StringJsonToInt(overallStatsJson["shotsToGoal"].ToString()));
            playerOverallStat.Add("Provoqued Drops", StringJsonToInt(overallStatsJson["provokedDrops"].ToString()));
            playerOverallStat.Add("Interceptions", StringJsonToInt(overallStatsJson["interceptions"].ToString()));
            playerOverallStat.Add("Possession Time", StringJsonToInt(overallStatsJson["possessionTime"].ToString()));
            playerOverallStat.Add("Bonuses Aquired", StringJsonToInt(overallStatsJson["bonusAquired"].ToString()));
            playerOverallStat.Add("Missiles Shot", StringJsonToInt(overallStatsJson["missilesShot"].ToString()));
            playerOverallStat.Add("Springs Used", StringJsonToInt(overallStatsJson["springsUsed"].ToString()));

            return playerOverallStat;
        }
    }
}
