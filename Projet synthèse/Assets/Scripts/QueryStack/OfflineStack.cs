using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.OfflineQueryStack
{
    class OfflineStack
    {
        public static string offlineStackDir;

        public void SaveToFile(JSONObject _JsonObject, string _filePath)
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            using (StreamWriter file = File.CreateText(_filePath))
            {
                file.Write(_JsonObject.ToString());
                file.Flush();
            }
        }

        public void StackGame(string _winningTeam, string _arenaId)
        {
            offlineStackDir = Application.dataPath + "/JSON/offlineStack/";
            JSONObject gameJsonObject = new JSONObject(JSONObject.Type.STRING);
            gameJsonObject.AddField("winner", _winningTeam);
            gameJsonObject.AddField("gameDate", DateTime.Today.ToString());
            gameJsonObject.AddField("gameTime", DateTime.Now.ToString());
            gameJsonObject.AddField("arenaID", _arenaId);

            int i = 1;
            string filePath = "";

            while (true)
            {
                filePath = offlineStackDir + "game" + i + ".json";
                if (!File.Exists(filePath))
                {
                    break;
                }
                i++;
            }

            SaveToFile(gameJsonObject, filePath);
        }

        public void StackUserStats(int _win, int _loss)
        {
            offlineStackDir = Application.dataPath + "/JSON/offlineStack/";
            JSONObject userStatsJson = new JSONObject(JSONObject.Type.STRING);
            userStatsJson.AddField("win", _win);
            userStatsJson.AddField("loss", _loss);
            userStatsJson.AddField("username", ConnectedUser.connectedUser.name.Replace("\"", ""));

            int i = 1;
            string filePath = "";

            while (true)
            {
                filePath = offlineStackDir + "userStats" + i + ".json";
                if (!File.Exists(filePath))
                {
                    break;
                }
                i++;
            }

            SaveToFile(userStatsJson, filePath);
        }

        public void StackGameStats(Dictionary<string, int> _gameStats, int _userId)
        {
            offlineStackDir = Application.dataPath + "/JSON/offlineStack/";
            JSONObject userStatsJson = new JSONObject(JSONObject.Type.STRING);
            userStatsJson.AddField("assists", _gameStats["Assists"]);
            userStatsJson.AddField("id", _userId);
            userStatsJson.AddField("goals", _gameStats["Goals"]);
            userStatsJson.AddField("deaths", _gameStats["Deaths"]);
            userStatsJson.AddField("shotsToGoal", _gameStats["Shots To Goal"]);
            userStatsJson.AddField("provoquedDrops", _gameStats["Provoqued Drops"]);
            userStatsJson.AddField("interceptions", _gameStats["Interceptions"]);
            userStatsJson.AddField("possessionTime", _gameStats["Possession Time"]);
            userStatsJson.AddField("bonusAquired", _gameStats["Bonuses Aquired"]);
            userStatsJson.AddField("missilesShot", _gameStats["Missiles Shot"]);
            userStatsJson.AddField("springsUsed", _gameStats["Springs Used"]);

            int i = 1;
            string filePath = "";

            while (true)
            {
                filePath = offlineStackDir + "gameStats" + i + ".json";
                if (!File.Exists(filePath))
                {
                    break;
                }
                i++;
            }

            SaveToFile(userStatsJson, filePath);
        }

        public void StackAchivements(int _achievementId, int _userId, int _progress)
        {
            offlineStackDir = Application.dataPath + "/JSON/offlineStack/";
            JSONObject achievementJson = new JSONObject(JSONObject.Type.STRING);
            achievementJson.AddField("progress", _progress);
            achievementJson.AddField("userId", _userId);
            achievementJson.AddField("achivementId", _achievementId);

            string filePath = offlineStackDir + "achievement" + _achievementId + ".json";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            SaveToFile(achievementJson, filePath);
        }
    }
}
