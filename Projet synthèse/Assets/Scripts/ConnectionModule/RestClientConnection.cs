using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ConnectionModule
{
    class RestClientConnection
    {
        private const string WEBSITE_URL = "http://duckhunters.webuda.com/";
        internal enum saveDirectory { USER, ARENA }

        private JSONObject CreateConnectionUserJsonObject(string _username, string _password)
        {
            string path = Application.dataPath + ConnectedUser.REF_PATH_FOR_USER_DIRRECTORY + _username + ".json";
            JSONObject userJson = new JSONObject(JSONObject.Type.STRING);
            userJson.AddField("password", _password);
            userJson.AddField("username", _username);
            return userJson;
        }

        private void setConnectedUser(JSONObject _userJsonObject)
        {
            User newUser = new User();
            newUser.id = Int32.Parse(_userJsonObject["id"].ToString().Replace("\"", ""));
            newUser.name = _userJsonObject["name"].ToString();
            newUser.password = _userJsonObject["password"].ToString();
            newUser.email = _userJsonObject["username"].ToString();
            newUser.avatarPath = _userJsonObject["AVATAR"].ToString();
            newUser.isOnline = true;
            newUser.totalNumberOfWins = Int32.Parse(_userJsonObject["WIN"].ToString().Replace("\"", ""));
            newUser.totalNumberOfLoses = Int32.Parse(_userJsonObject["LOSS"].ToString().Replace("\"", ""));
            newUser.InitialiseAttributes();
            ConnectedUser.connectedUser = newUser;
        }
        public void SaveToFile(JSONObject _jsonObject, string _fileName, saveDirectory _directory)
        {
            string directory = "";

            if (_directory == saveDirectory.USER)
            {
                directory = Application.dataPath + ConnectedUser.REF_PATH_FOR_USER_DIRRECTORY;
            }
            else
            {
                directory = Application.dataPath + ConnectionModule.ARENA_DIRECTORY;
            }

            string path = directory + _fileName + ".json";
            if (System.IO.File.Exists(path))
            {
                File.Delete(directory + _fileName + ".json");
            }

            using (StreamWriter file = File.CreateText(directory + _fileName + ".json"))
            {
                file.Write(_jsonObject.ToString());
                file.Flush();
            }
        }

        public string TryLogin(string _username, string _passWord, ConnectionModule.HttpResquest httpResquestType)
        {
            const string URL = WEBSITE_URL + "ConnectionApi.php";

            JSONObject userJsonObject = CreateConnectionUserJsonObject(_username, _passWord);
            WebRequest request;
            WebResponse response;
            Stream dataStream;
            StreamReader reader;
            string responseFromServer = "";

            try
            {
                if (httpResquestType == ConnectionModule.HttpResquest.GET)
                {
                    string urlGet = URL + "?username=" + _username + "&password=" + _passWord;

                    request = WebRequest.Create(urlGet);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    response = request.GetResponse();
                    Debug.Log(((HttpWebResponse)response).StatusDescription);
                    dataStream = response.GetResponseStream();
                    reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    Debug.Log(responseFromServer);
                    reader.Close();
                    response.Close();
                }
                if (httpResquestType == ConnectionModule.HttpResquest.POST)
                {

                    request = WebRequest.Create(URL);
                    request.Method = "POST";
                    byte[] byteArray = Encoding.UTF8.GetBytes(userJsonObject.ToString());
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
            catch (Exception)
            {

                return ConnectedUser.FAIL_TO_CONNECT;
            }

            //Remove junk from web hoster
            if (responseFromServer.Contains("http://stats.hosting24.com/count.php"))
            {
                int lastChar = responseFromServer.IndexOf("\r");
                responseFromServer = responseFromServer.Substring(0, lastChar);
            }

            if (responseFromServer.Contains("username\":\"" + _username + "\"") && responseFromServer.Contains("password\":\"" + _passWord + "\""))
            {
                userJsonObject = new JSONObject(responseFromServer);
                SaveToFile(userJsonObject, _username, saveDirectory.USER);
                setConnectedUser(userJsonObject);
                return ConnectedUser.LOGIN_SUCCESFUL;
            }
            return ConnectedUser.FAIL_TO_LOGIN;
        }

        public IList<Arena> GetAllArenaOnDB(ConnectionModule.HttpResquest httpResquestType)
        {
            List<Arena> arenas = new List<Arena>();

            const string URL = WEBSITE_URL + "getArenaApi.php";

            JSONObject arenaJsonObject = new JSONObject(JSONObject.Type.STRING);
            WebRequest request;
            WebResponse response;
            Stream dataStream;
            StreamReader reader;
            string responseFromServer = "";
            request = WebRequest.Create(URL);

            if (httpResquestType == ConnectionModule.HttpResquest.POST)
            {
                request.Method = "POST";

            }
            else if (httpResquestType == ConnectionModule.HttpResquest.PUT)
            {
                request.Method = "PUT";
            }

            if (httpResquestType != ConnectionModule.HttpResquest.GET)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(arenaJsonObject.ToString());
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

            if (responseFromServer.Contains("http://stats.hosting24.com/count.php"))
            {
                int lastChar = responseFromServer.IndexOf("\r");
                responseFromServer = responseFromServer.Substring(0, lastChar);
            }

            int responseLenght = responseFromServer.Length;
            string currentArenaBuilder = "";
            for (int i = 0; i < responseLenght; i++)
            {
                currentArenaBuilder += responseFromServer[i];
                if (responseFromServer[i] == '}')
                {
                    arenaJsonObject = new JSONObject(currentArenaBuilder);
                    Arena currentArena = new Arena();
                    string idStr = arenaJsonObject[0].ToString();
                    string id = "";

                    for (int j = 0; j < idStr.Length; j++)
                    {
                        int result;
                        if (int.TryParse(idStr[j].ToString(), out result))
                        {
                            id += result;
                        }
                    }
                    currentArena.id = int.Parse(id);
                    currentArena.name = arenaJsonObject[1].ToString();
                    currentArena.description = arenaJsonObject[2].ToString();
                    currentArena.imagePath = arenaJsonObject[3].ToString();
                    SaveToFile(arenaJsonObject, currentArena.id.ToString(), saveDirectory.ARENA);

                    arenas.Add(currentArena);
                    currentArenaBuilder = "";
                }
            }
            return arenas;
        }
    }
}
