using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.AchievementModule;
using Assets.Scripts.GameStatisticsModule;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    [Header("The textbox containing the player's stats.")]
    public Text gameStats;

    [Header("The panel containing the list of achievement.")]
    public GameObject achievementList;

    [Header("The achievement profab to instantiate.")]
    public GameObject achievementPrefab;

    private string formattedImageUrl;
    private string formattedName;
    private string formattedObjective;
    private const int DISTANCE_BETWEEN_ACHIEVEMENTS = 225;
    private OnlineGameStatistics gameStatisticsModule;
    private Dictionary<string, Image> imageList = new Dictionary<string, Image>();
    private Dictionary<string, WWW> wwwList = new Dictionary<string, WWW>();
    private const string SERVER_URL = "http://duckhunters.webuda.com";

    void Start()
    {
        gameStats.text = GetPlayerStats();

        GetUserAchievements();
        if (ConnectedUser.connectedUser.isOnline)
        {
            CreateWWWObjectsForPictures();
        }


    }

    private String GetPlayerStats()
    {
        string text = "";

        if (ConnectedUser.connectedUser != null)
        {
            foreach (KeyValuePair<string, int> keyValuePair in ConnectedUser.connectedUser.GetPlayerStatsFile())
            {
                text += keyValuePair.Key + ": " + keyValuePair.Value.ToString();
                if (keyValuePair.Key == "Possession Time: ")
                {
                    text += " seconds ";
                }
                text += "\n";
            }
        }

        return text;
    }

    private void GetUserAchievements()
    {
        GameObject achievementToAdd;

        foreach (Achievement achievement in ConnectedUser.connectedUser.UserAchievementsList)
        {
            achievementToAdd = Instantiate(achievementPrefab);

            //Set the achievement's title's text component
            formattedName = achievement.name;
            formattedName = formattedName.Replace("\"", "");
            achievementToAdd.transform.GetChild(0).GetComponent<Text>().text = achievement.name;

            //Set the achievement's objective's text component
            formattedObjective = achievement.objective;
            formattedObjective = formattedObjective.Replace("\"", "");
            achievementToAdd.transform.GetChild(1).GetComponent<Text>().text = achievement.objective;

            //Fill the progress bar and display the progress in percentage
            achievementToAdd.transform.GetChild(2).GetComponent<Image>().fillAmount = ((float)achievement.progress / 100);
            achievementToAdd.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = achievement.progress + "%";

            //Add the picture in a list to later display once the associated WWW object is ready
            Image imageToSet = achievementToAdd.transform.GetChild(3).GetComponent<Image>();
            imageList.Add(formattedName, imageToSet);

            //Set the achievement's position
            achievementToAdd.transform.SetParent(achievementList.transform);
            if (achievementList.transform.childCount == 1)
            {
                achievementToAdd.transform.position = new Vector3(achievementList.transform.position.x,
                    achievementList.transform.position.y, achievementList.transform.position.z);
            }
            else
            {
                achievementToAdd.transform.position = new Vector3(achievementList.transform.position.x,
                    achievementList.transform.GetChild(achievementList.transform.childCount - 2).position.y -
                    DISTANCE_BETWEEN_ACHIEVEMENTS, achievementList.transform.position.z);
            }
        }

    }

    private void CreateWWWObjectsForPictures()
    {
        foreach (KeyValuePair<string, Image> keyValuePair in imageList)
        {
            formattedImageUrl = Achievement.GetAchievementByName(keyValuePair.Key).image;
            formattedImageUrl = formattedImageUrl.Replace("\"", "");
            formattedImageUrl = formattedImageUrl.Replace("\\", "");
            WWW www = new WWW(SERVER_URL + "/" + formattedImageUrl);
            wwwList.Add(keyValuePair.Key, www);
        }
    }

    private void DisplayPictures()
    {
        try
        {
            foreach (KeyValuePair<string, WWW> keyValuePair in wwwList)
            {
                if (keyValuePair.Value != null && keyValuePair.Value.isDone)
                {
                    Material material = new Material(imageList[keyValuePair.Key].material);
                    material.mainTexture = keyValuePair.Value.texture;
                    imageList[keyValuePair.Key].material = material;
                    wwwList[keyValuePair.Key] = null;
                }
            }
        }
        catch
        {

        }
    }

    void Update()
    {
        DisplayPictures();
    }

}
