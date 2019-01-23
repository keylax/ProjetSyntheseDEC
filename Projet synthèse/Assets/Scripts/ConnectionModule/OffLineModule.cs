using System.Collections.Generic;
using System.IO;
using Assets.Scripts.AchievementModule;
using Assets.Scripts.GameStatisticsModule;
using UnityEngine;


namespace Assets.Scripts.ConnectionModule
{
    public class OfflineModule : ConnectionModule
    {

        private bool AccountExists(string _userName)
        {
            string path = ConnectedUser.REF_PATH_FOR_USER_DIRRECTORY + _userName + ".json";

            if (System.IO.File.Exists(path))
            {
                Debug.Log("UserName Exist");
                return true;
            }
            Debug.Log("Username does not exist.");
            return false;
        }

        public static string GetDataFomJson(JSONObject _jsonObject, string _data)
        {
            Debug.Log(_data);
            string jsonData = _jsonObject[_data].ToString();
            jsonData = jsonData.Replace("\"", "");
            jsonData.Trim();
            return jsonData;
        }

        private bool ValidatePassword(string _userName, string _password)
        {

            string path = ConnectedUser.REF_PATH_FOR_USER_DIRRECTORY + _userName + ".json";
            string jsonContent = File.ReadAllText(path);

            JSONObject json = new JSONObject(jsonContent);

            string jsonPassword = GetDataFomJson(json, "password");

            if (jsonPassword == _password)
            {
                Debug.Log("Password is valid");
                return true;
            }
            Debug.Log("Password is not valid");
            return false;
        }

        private void SetCurrentUser(string _userName)
        {
            string path = ConnectedUser.REF_PATH_FOR_USER_DIRRECTORY + _userName + ".json";
            string jsonContent = File.ReadAllText(path);

            JSONObject json = new JSONObject(jsonContent);
            ConnectedUser.connectedUser = new User();

            ConnectedUser.connectedUser.id = int.Parse(GetDataFomJson(json, "id"));
            ConnectedUser.connectedUser.name = GetDataFomJson(json, "name");
            ConnectedUser.connectedUser.password = GetDataFomJson(json, "password");
            ConnectedUser.connectedUser.email = _userName;
            ConnectedUser.connectedUser.totalNumberOfWins = int.Parse(GetDataFomJson(json, "WIN"));
            ConnectedUser.connectedUser.totalNumberOfLoses = int.Parse(GetDataFomJson(json, "LOSS"));
            ConnectedUser.connectedUser.isOnline = false;
            ConnectedUser.connectedUser.InitialiseAttributes();
        }

        private void setArenas()
        {

            string jsonContent = "";
            JSONObject currentArenaJsonObject = new JSONObject(JSONObject.Type.STRING);
            Arena currentArena = new Arena();
            GameParameters.ArenaList = new List<Arena>();

            string path = Application.dataPath + ARENA_DIRECTORY;
            string[] files;
            files = Directory.GetFiles(path, "*.json");

            for (int i = 0; i < files.Length; i++)
            {
                jsonContent = File.ReadAllText(files[i]);
                currentArenaJsonObject = new JSONObject(jsonContent);
                currentArena.id = i;
                string name = currentArenaJsonObject["name"].ToString();
                name = name.Replace("\"", "");

                currentArena.name = name;
                currentArena.description = GetDataFomJson(currentArenaJsonObject, "description");

                GameParameters.ArenaList.Add(currentArena);
                currentArena = new Arena();
            }
        }

        private Dictionary<string, int> getGlobalStats()
        {
            string jsonContent = "";
            JSONObject globalJsonObject = new JSONObject(JSONObject.Type.STRING);
            string fileName = Application.dataPath + "/JSON/overallStats/" + "overallStats" + ConnectedUser.connectedUser.id + ".json";


            if (File.Exists(fileName))
            {
                jsonContent = File.ReadAllText(fileName);
                globalJsonObject = new JSONObject(jsonContent);

                Dictionary<string, int> playerOverallStat = new Dictionary<string, int>();

                playerOverallStat.Add("Assists", int.Parse(GetDataFomJson(globalJsonObject, "assists")));
                playerOverallStat.Add("Goals", int.Parse(GetDataFomJson(globalJsonObject, "id")));
                playerOverallStat.Add("Deaths", int.Parse(GetDataFomJson(globalJsonObject, "goals")));
                playerOverallStat.Add("Shots To Goal", int.Parse(GetDataFomJson(globalJsonObject, "deaths")));
                playerOverallStat.Add("Provoqued Drops", int.Parse(GetDataFomJson(globalJsonObject, "shotsToGoal")));
                playerOverallStat.Add("Interceptions", int.Parse(GetDataFomJson(globalJsonObject, "provokedDrops")));
                playerOverallStat.Add("Possession Time", int.Parse(GetDataFomJson(globalJsonObject, "possessionTime")));
                playerOverallStat.Add("Bonuses Aquired", int.Parse(GetDataFomJson(globalJsonObject, "bonusAquired")));

                return playerOverallStat;
            }

            return new Dictionary<string, int>();
        }

        public override string Connect(string _userName, string _password)
        {
            if (AccountExists(_userName) && ValidatePassword(_userName, _password))
            {
                SetCurrentUser(_userName);
                OfflineAchivements offlineAchivements = new OfflineAchivements();
                //Set General Achievements
                offlineAchivements.SetAchievementsList(false, -1);
                //Set user achievements
                offlineAchivements.SetAchievementsList(true, ConnectedUser.connectedUser.id);
                setArenas();

                return ConnectedUser.LOGIN_SUCCESFUL;
            }
            return ConnectedUser.FAIL_TO_LOGIN;
        }
    }
}


