using System.Collections.Generic;
using System.IO;
using Assets.Scripts.AchievementModule;
using Assets.Scripts.GameController;
using Assets.Scripts.GameStatisticsModule;
using UnityEngine;

public class User
{
    public bool isOnline = false;
    public int id { get; set; }
    public string name { get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public string avatarPath { get; set; }
    public IList<Achievement> UserAchievementsList { get; set; }

    //Ces statistiques sont les stats globales (exemple nombre total de but que le joueur a compté dans sa carrière), elles sont donc différentes
    //des statistiques d'une seule partie dans la classe CurrentGame.
    public int totalNumberOfGoals { get; set; }
    public int totalNumberOfDeaths { get; set; }
    public int totalNumberOfShotsToGoal { get; set; }
    public int totalNumberOfProvoquedDrops { get; set; }
    public int totalNumberOfInterceptions { get; set; }
    public int totalPossessionTime { get; set; }
    public int totalNumberOfBonusesAquired { get; set; }
    public int totalNumberOfAssists { get; set; }
    public int totalNumberOfWins { get; set; }
    public int totalNumberOfLoses { get; set; }
    public int totalNumberOfGamePlayed { get; set; }
    public int totalNumberOfMissilesShot { get; set; }
    public int totalNumberOfJumpsFromSprings { get; set; }
    public int numberOfQuickMatchWonInARow { get; set; }

    private Dictionary<string, int> playerOverallStats { get; set; }

    private OnlineGameStatistics gameStatisticsModule;

    public void InitialiseAttributes()
    {
        if (isOnline)
        {
            gameStatisticsModule = new OnlineGameStatistics();
            playerOverallStats = gameStatisticsModule.GetOverallStats(id);

            SaveStats();
        }
        else
        {
            playerOverallStats = GetPlayerStatsFile();
        }
        totalNumberOfGoals = playerOverallStats["Goals"];
        totalNumberOfDeaths = playerOverallStats["Deaths"];
        totalNumberOfInterceptions = playerOverallStats["Interceptions"];
        totalNumberOfBonusesAquired = playerOverallStats["Bonuses Aquired"];
        totalNumberOfShotsToGoal = playerOverallStats["Shots To Goal"];
        totalNumberOfProvoquedDrops = playerOverallStats["Provoqued Drops"];
        totalNumberOfAssists = playerOverallStats["Assists"];
        totalPossessionTime = playerOverallStats["Possession Time"];
        totalNumberOfMissilesShot = playerOverallStats["Missiles Shot"];
        totalNumberOfJumpsFromSprings = playerOverallStats["Springs Used"];
        numberOfQuickMatchWonInARow = 0;
        totalNumberOfGamePlayed = totalNumberOfWins + totalNumberOfLoses;
    }

    public void SaveStats()
    {
        JSONObject stats = new JSONObject(JSONObject.Type.STRING);

        stats.AddField("Wins", totalNumberOfWins);
        stats.AddField("Losses", totalNumberOfLoses);
        stats.AddField("Number of games", totalNumberOfWins + totalNumberOfLoses);

        foreach (KeyValuePair<string, int> keyValuePair in playerOverallStats)
        {
            stats.AddField(keyValuePair.Key, keyValuePair.Value);
        }

        SaveToFile(stats, name.Replace("\"", ""), "/JSON/PlayerStats/");
    }

    private void SaveToFile(JSONObject _jsonObject, string _fileName, string _directory)
    {
        string path = Application.dataPath + _directory + _fileName + ".json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using (StreamWriter file = File.CreateText(path))
        {
            file.Write(_jsonObject.ToString());
            file.Flush();
        }
    }

    public Dictionary<string, int> GetPlayerStatsFile()
    {
        Dictionary<string, int> statsDictionary = new Dictionary<string, int>();
        string path = Application.dataPath + "/JSON/PlayerStats/" + name.Replace("\"", "") + ".json";
        JSONObject statsFile = new JSONObject(File.ReadAllText(path));

        statsDictionary.Add("Number of Games", int.Parse(statsFile["Number of games"].ToString()));
        statsDictionary.Add("Wins", int.Parse(statsFile["Wins"].ToString()));
        statsDictionary.Add("Losses", int.Parse(statsFile["Losses"].ToString()));
        statsDictionary.Add("Goals", int.Parse(statsFile["Goals"].ToString()));
        statsDictionary.Add("Assists", int.Parse(statsFile["Assists"].ToString()));
        statsDictionary.Add("Deaths", int.Parse(statsFile["Deaths"].ToString()));
        statsDictionary.Add("Shots To Goal", int.Parse(statsFile["Shots To Goal"].ToString()));
        statsDictionary.Add("Provoqued Drops", int.Parse(statsFile["Provoqued Drops"].ToString()));
        statsDictionary.Add("Interceptions", int.Parse(statsFile["Interceptions"].ToString()));
        statsDictionary.Add("Possession Time", int.Parse(statsFile["Possession Time"].ToString()));
        statsDictionary.Add("Bonuses Aquired", int.Parse(statsFile["Bonuses Aquired"].ToString()));
        statsDictionary.Add("Missiles Shot", int.Parse(statsFile["Missiles Shot"].ToString()));
        statsDictionary.Add("Springs Used", int.Parse(statsFile["Springs Used"].ToString()));

        return statsDictionary;
    }

    public void AddCurrentGameStatsToTotalStats()
    {
        totalNumberOfGoals += CurrentGame.GetPlayer1Goals().Count;
        totalNumberOfDeaths += CurrentGame.Statistics.NumberOfDeaths;
        totalNumberOfInterceptions += CurrentGame.Statistics.NumberOfInterceptions;
        totalNumberOfBonusesAquired += CurrentGame.Statistics.NumberOfBonusesAquired;
        totalNumberOfShotsToGoal += CurrentGame.Statistics.NumberOfShotsToGoal;
        totalNumberOfProvoquedDrops += CurrentGame.Statistics.NumberOfProvoquedDrops;
        totalNumberOfAssists += CurrentGame.Statistics.NumberOfAssists;
        totalPossessionTime += (int)CurrentGame.Statistics.PossessionTime;
        totalNumberOfMissilesShot += CurrentGame.Statistics.NumberOfMissilesShot;
        totalNumberOfJumpsFromSprings += CurrentGame.Statistics.NumberOfJumpsFromSprings; 
    }

}
