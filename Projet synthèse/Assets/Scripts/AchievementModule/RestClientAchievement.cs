using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AchievementModule
{
    class RestClientAchievement
    {
        private const string WEB_URL = "http://duckhunters.webuda.com/";

        public JSONObject CreateAchievementJsonObject(int _achievementId, int _userId, int _progress)
        {
            JSONObject achievementJson = new JSONObject(JSONObject.Type.NUMBER);
            achievementJson.AddField("progress", _progress);
            achievementJson.AddField("userId", _userId);
            achievementJson.AddField("achievementId", _achievementId);
            return achievementJson;
        }

        private int StringJsonToInt(string jsonString)
        {
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

        public void SaveToFile(JSONObject _JsonObject, string _fileName, bool _isUserAchiement)
        {
            string directory = "";

            if (_isUserAchiement)
            {
                directory = Application.dataPath + AchievementModule.USER_ACHIEVEMENT_DIRECTORY;
            }
            else
            {
                directory = Application.dataPath + AchievementModule.ALL_ACHIEVEMENT_DIRECTORY;
            }

            string path = directory + _fileName + ".json";
            if (File.Exists(path))
            {
                File.Delete(directory + _fileName + ".json");
            }

            using (StreamWriter file = File.CreateText(directory + _fileName + ".json"))
            {
                file.Write(_JsonObject.ToString());
                file.Flush();
            }
        }

        public void UpdateDB(JSONObject _jsonObject, string _api, ConnectionModule.ConnectionModule.HttpResquest httpResquestType)
        {
            string url = WEB_URL + _api;

            WebRequest request;
            WebResponse response;
            Stream dataStream;
            StreamReader reader;
            string responseFromServer = "";
            request = WebRequest.Create(url);


            if (httpResquestType == ConnectionModule.ConnectionModule.HttpResquest.POST)
            {
                request.Method = "POST";

            }
            else if (httpResquestType == ConnectionModule.ConnectionModule.HttpResquest.PUT)
            {
                request.Method = "PUT";
            }

            if (httpResquestType != ConnectionModule.ConnectionModule.HttpResquest.GET)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(_jsonObject.ToString());
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                response = request.GetResponse();
                Debug.Log(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                Debug.Log(responseFromServer);
                reader.Close();
                dataStream.Close();
                response.Close();
            }

        }

        public IList<Achievement> GetAchievementsFromDB(string _api, int _userid, ConnectionModule.ConnectionModule.HttpResquest httpResquestType)
        {
            string url = WEB_URL + _api;

            List<Achievement> achivements = new List<Achievement>();
            JSONObject jsonObject = new JSONObject(JSONObject.Type.STRING);

            jsonObject.AddField("id", _userid);

            WebRequest request;
            WebResponse response;
            Stream dataStream;
            StreamReader reader;
            string responseFromServer = "";
            request = WebRequest.Create(url);


            if (httpResquestType == ConnectionModule.ConnectionModule.HttpResquest.POST)
            {
                request.Method = "POST";

            }
            else if (httpResquestType == ConnectionModule.ConnectionModule.HttpResquest.PUT)
            {
                request.Method = "PUT";
            }

            if (httpResquestType != ConnectionModule.ConnectionModule.HttpResquest.GET)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonObject.ToString());
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                response = request.GetResponse();
                Debug.Log(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                Debug.Log(responseFromServer);
                reader.Close();
                dataStream.Close();
                response.Close();
            }


            int responseLenght = responseFromServer.Length;
            string currentArenaBuilder = "";
            for (int i = 0; i < responseLenght; i++)
            {
                currentArenaBuilder += responseFromServer[i];
                if (responseFromServer[i] == '}')
                {
                    jsonObject = new JSONObject(currentArenaBuilder);
                    Achievement currentAchievement = new Achievement();

                    currentAchievement.id = StringJsonToInt(jsonObject["id"].ToString());
                    currentAchievement.name = jsonObject["name"].ToString();
                    currentAchievement.image = jsonObject["image"].ToString();
                    currentAchievement.objective = jsonObject["objective"].ToString();
                    currentAchievement.points = StringJsonToInt(jsonObject["points"].ToString());
                    currentAchievement.progress = StringJsonToInt(jsonObject["progress"].ToString());
                    if (_api == "getAllAchievements.php")
                    {
                        SaveToFile(jsonObject, currentAchievement.id.ToString(), false);
                    }
                    else
                    {
                        SaveToFile(jsonObject, currentAchievement.id.ToString(), true);
                    }

                    achivements.Add(currentAchievement);
                    currentArenaBuilder = "";
                }
            }
            return achivements;
        }
    }
}