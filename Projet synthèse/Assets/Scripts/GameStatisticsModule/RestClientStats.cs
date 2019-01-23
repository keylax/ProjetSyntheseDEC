using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStatisticsModule
{
    class RestClientStats
    {
        private const string WEB_URL = "http://duckhunters.webuda.com/";

        public JSONObject CreateJsonForGame(string _winningTeam, int _arenaId)
        {
            JSONObject userStatsJson = new JSONObject(JSONObject.Type.STRING);

            userStatsJson.AddField("winner", _winningTeam);
            userStatsJson.AddField("gameDate", DateTime.Today.ToString());
            userStatsJson.AddField("gameTime", DateTime.Now.ToString());
            userStatsJson.AddField("arenaID", _arenaId);
            return userStatsJson;
        }

        public JSONObject CreateJsonForPlayerStats(int _win, int _loss, string _usename)
        {
            JSONObject userStatsJson = new JSONObject(JSONObject.Type.STRING);
            userStatsJson.AddField("win", _win);
            userStatsJson.AddField("loss", _loss);
            userStatsJson.AddField("username", _usename);
            return userStatsJson;
        }

        public JSONObject CreateJsonForGameStats(Dictionary<string, int> _playerOverallStat, int _userId)
        {
            JSONObject userStatsJson = new JSONObject(JSONObject.Type.STRING);
            userStatsJson.AddField("assists", _playerOverallStat["Assists"]);
            userStatsJson.AddField("userId", _userId);
            userStatsJson.AddField("goals", _playerOverallStat["Goals"]);
            userStatsJson.AddField("deaths", _playerOverallStat["Deaths"]);
            userStatsJson.AddField("shotsToGoal", _playerOverallStat["Shots To Goal"]);
            userStatsJson.AddField("provokedDrops", _playerOverallStat["Provoqued Drops"]);
            userStatsJson.AddField("interceptions", _playerOverallStat["Interceptions"]);
            userStatsJson.AddField("possessionTime", _playerOverallStat["Possession Time"]);
            userStatsJson.AddField("bonusAquired", _playerOverallStat["Bonuses Aquired"]);
            userStatsJson.AddField("missilesShot", _playerOverallStat["Missiles Shot"]);
            userStatsJson.AddField("springsUsed", _playerOverallStat["Springs Used"]);

            string fileName = "overallStats";

            string path = Application.dataPath + "/JSON/overallStats/" + fileName + ".json";
            if (System.IO.File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter file = File.CreateText(path))
            {
                file.Write(userStatsJson.ToString());
                file.Flush();
            }


            return userStatsJson;
        }

        public JSONObject CreateJsonForUserOverallStats(int _userId)
        {
            JSONObject userStatsJson = new JSONObject(JSONObject.Type.STRING);
            userStatsJson.AddField("id", _userId);
            return userStatsJson;
        }


        public JSONObject UpdateDB(JSONObject _jsonObject, string _api, ConnectionModule.ConnectionModule.HttpResquest httpResquestType)
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

                try
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
                catch
                {
                    return new JSONObject();
                }

            }

            //Remove junk from web hoster
            if (responseFromServer.Contains("http://stats.hosting24.com/count.php"))
            {
                int lastChar = responseFromServer.IndexOf("\r");
                responseFromServer = responseFromServer.Substring(0, lastChar);
            }


            _jsonObject = new JSONObject(responseFromServer);
            return _jsonObject;
        }
    }
}