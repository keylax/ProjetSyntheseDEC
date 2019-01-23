using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.AchievementModule;
using Assets.Scripts.ConnectionModule;
using Assets.Scripts.GameStatisticsModule;
using Assets.Scripts.OfflineQueryStack;
using UnityEngine;

namespace Assets.Scripts.QueryStack
{
    class OnlineStack
    {
        AchievementModule.AchievementModule achievementModule = new OnlineAchievement();
        GameStatisticsModule.GameStatisticsModule gameStatisticsModule = new OnlineGameStatistics();

        private void MakeStackGameQueries()
        {
            string path = Application.dataPath + "/JSON/offlineStack/";
            string[] files;
            files = Directory.GetFiles(path, "game*.json");

            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Contains("gameStats"))
                {
                    string jsonContent = File.ReadAllText(files[i]);
                    JSONObject gameJsonObject = new JSONObject(jsonContent);
                    gameStatisticsModule.UpdateGame(OfflineModule.GetDataFomJson(gameJsonObject, "winner"), int.Parse(OfflineModule.GetDataFomJson(gameJsonObject, "arenaID")), ConnectionModule.ConnectionModule.HttpResquest.POST);

                    File.Delete(files[i]);
                }
            }
        }

        private void MakeStackGameStatsQueries()
        {
            string path = Application.dataPath + "/JSON/offlineStack/";
            string[] files;
            files = Directory.GetFiles(path, "gameStats*.json");
            for (int i = 0; i < files.Length; i++)
            {
                string jsonContent = File.ReadAllText(files[i]);
                JSONObject gameStatsJsonObject = new JSONObject(jsonContent);

                Dictionary<string, int> playerOverallStat = new Dictionary<string, int>();

                playerOverallStat.Add("Assists", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "assists")));
                playerOverallStat.Add("userId", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "id")));
                playerOverallStat.Add("Goals", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "goals")));
                playerOverallStat.Add("Deaths", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "deaths")));
                playerOverallStat.Add("Shots To Goal", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "shotsToGoal")));
                playerOverallStat.Add("Provoqued Drops", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "provoquedDrops")));
                playerOverallStat.Add("Interceptions", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "interceptions")));
                playerOverallStat.Add("Possession Time", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "possessionTime")));
                playerOverallStat.Add("Bonuses Aquired", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "bonusAquired")));
                playerOverallStat.Add("Missiles Shot", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "missilesShot")));
                playerOverallStat.Add("Springs Used", int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "springsUsed")));
                gameStatisticsModule.UpdateGameStats(playerOverallStat, int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "id")));

                File.Delete(files[i]);
            }
        }

        private void MakeStackUserStatsQueries()
        {
            string path = Application.dataPath + "/JSON/offlineStack/";
            string[] files;
            files = Directory.GetFiles(path, "userStats*.json");
            for (int i = 0; i < files.Length; i++)
            {
                string jsonContent = File.ReadAllText(files[i]);
                JSONObject gameStatsJsonObject = new JSONObject(jsonContent);
                int win = int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "win"));
                int loss = int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "loss"));
                string userName = OfflineModule.GetDataFomJson(gameStatsJsonObject, "username");
                gameStatisticsModule.UpdateUserStats(win, loss, userName);

                File.Delete(files[i]);
            }
        }

        private void MakeStackAchievementQueries()
        {
            string path = Application.dataPath + "/JSON/offlineStack/";
            string[] files;
            files = Directory.GetFiles(path, "achievement*.json");
            for (int i = 0; i < files.Length; i++)
            {
                string jsonContent = File.ReadAllText(files[i]);
                JSONObject gameStatsJsonObject = new JSONObject(jsonContent);
                int achievementId = int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "achivementId"));
                int userId = int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "userId"));
                int progress = int.Parse(OfflineModule.GetDataFomJson(gameStatsJsonObject, "progress"));
                achievementModule.UpdateAchievement(achievementId, userId, progress);

                File.Delete(files[i]);
            }
        }

        public void EmptyStack()
        {
            MakeStackGameQueries();
            MakeStackGameStatsQueries();
            MakeStackUserStatsQueries();
            MakeStackAchievementQueries();
        }

    }
}
