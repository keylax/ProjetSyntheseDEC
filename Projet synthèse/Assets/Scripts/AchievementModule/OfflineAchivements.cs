using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.AchievementModule
{
    class OfflineAchivements
    {

        private string GetDataFomJson(JSONObject _jsonObject, string _data)
        {
            string jsonData = _jsonObject[_data].ToString();
            jsonData = jsonData.Replace("\"", "");
            jsonData.Trim();
            return jsonData;
        }


        public void SetAchievementsList(bool _isUser, int _userId)
        {
            string path = "";
            string progress;
            List<Achievement> achivementList = new List<Achievement>();
            string[] files;

            if (_isUser)
            {
                path = Application.dataPath + AchievementModule.USER_ACHIEVEMENT_DIRECTORY;
            }
            else
            {
                path = Application.dataPath + AchievementModule.ALL_ACHIEVEMENT_DIRECTORY;
            }

            files = Directory.GetFiles(path, "*.json");

            for (int i = 0; i < files.Length; i++)
            {
                string jsonContent = File.ReadAllText(files[i]);
                JSONObject jsonObject = new JSONObject(jsonContent);
                Achievement currentAchievement = new Achievement();
                currentAchievement.id = int.Parse(GetDataFomJson(jsonObject, "id"));
                currentAchievement.name = GetDataFomJson(jsonObject, "name");
                currentAchievement.objective = GetDataFomJson(jsonObject, "objective");
                currentAchievement.points = int.Parse(GetDataFomJson(jsonObject, "points"));
                if (GetDataFomJson(jsonObject, "progress") == "null")
                {
                    currentAchievement.progress = 0;
                }
                else
                {
                    currentAchievement.progress = int.Parse(GetDataFomJson(jsonObject, "progress"));
                }

                achivementList.Add(currentAchievement);
            }

            if (_isUser)
            {
                ConnectedUser.connectedUser.UserAchievementsList = achivementList;
            }
            else
            {
                GameParameters.AchievementsList = achivementList;
            }

        }
    }
}
