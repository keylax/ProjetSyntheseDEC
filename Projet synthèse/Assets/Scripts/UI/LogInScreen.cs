using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets.Scripts.ConnectionModule;
using UnityEngine.UI;

public class LogInScreen : MonoBehaviour
{
    [Header("Textbox containing the username.")]
    public InputField username;

    [Header("Textbox containing the password.")]
    public InputField password;

    [Header("Guest checkbox")]
    public Toggle isGuest;

    [Header("The error label to display when an error occurs")]
    public Text errorMsg;

    private ConnectionModule connectionModule;

    void Start()
    {
        SetVolumes(File.Exists(Application.dataPath + "/soundConfig.txt"));
        errorMsg.gameObject.SetActive(false);
        if (!Directory.Exists(Application.dataPath + "/JSON"))
        {
            Directory.CreateDirectory(Application.dataPath + "/JSON");
            Directory.CreateDirectory(Application.dataPath + "/JSON/users");
            Directory.CreateDirectory(Application.dataPath + "/JSON/all_achievements");
            Directory.CreateDirectory(Application.dataPath + "/JSON/user_achievements");
            Directory.CreateDirectory(Application.dataPath + "/JSON/arenas");
            Directory.CreateDirectory(Application.dataPath + "/JSON/Editor");
            Directory.CreateDirectory(Application.dataPath + "/JSON/PlayerStats");
            Directory.CreateDirectory(Application.dataPath + "/JSON/offlineStack");
            Directory.CreateDirectory(Application.dataPath + "/JSON/overallStats");
        }
        else
        {
            if (!Directory.Exists(Application.dataPath + "/JSON/users"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JSON/users");
            }

            if (!Directory.Exists(Application.dataPath + "/JSON/all_achievements"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JSON/all_achievements");
            }

            if (!Directory.Exists(Application.dataPath + "/JSON/user_achievements"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JSON/user_achievements");
            }

            if (!Directory.Exists(Application.dataPath + "/JSON/arenas"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JSON/arenas");
            }

            if (!Directory.Exists(Application.dataPath + "/JSON/Editor"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JSON/Editor");
            }

            if (!Directory.Exists(Application.dataPath + "/JSON/PlayerStats"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JSON/PlayerStats");
            }

            if (!Directory.Exists(Application.dataPath + "/JSON/offlineStack"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JSON/offlineStack");
            }

            if (!Directory.Exists(Application.dataPath + "/JSON/overallStats"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JSON/overallStats");
            }
        }

        SoundManager.PlaySFX("GameName");
    }

    private void SetVolumes(bool _volumeSettingsFileExists)
    {
        if (_volumeSettingsFileExists)
        {
            StreamReader soundSettingsReader = new StreamReader(Application.dataPath + "/soundConfig.txt");

            GameParameters.MasterVolume = Convert.ToSingle(soundSettingsReader.ReadLine());
            GameParameters.MusicVolume = Convert.ToSingle(soundSettingsReader.ReadLine());
            GameParameters.SoundEffectsVolume = Convert.ToSingle(soundSettingsReader.ReadLine());
            GameParameters.AnnouncerVolume = Convert.ToSingle(soundSettingsReader.ReadLine());

            soundSettingsReader.Close();
        }
        else
        {
            Debug.Log("Failed to load sound settings. Setting default settings.");

            GameParameters.MasterVolume = 1f;
            GameParameters.MusicVolume = 1f;
            GameParameters.SoundEffectsVolume = 1f;
            GameParameters.AnnouncerVolume = 1f;
        }

        SoundManager.SetVolumeMusic(GameParameters.MusicVolume);
        SoundManager.SetVolume(GameParameters.MasterVolume);
        SoundManager.SetVolumeSFX(GameParameters.SoundEffectsVolume);
    }

    public void LogIn()
    {

        if (isGuest != null && isGuest.isOn)
        {
            ConnectedUser.connectedUser = null;
            InitialiseArenaList();
            Application.LoadLevel("MainMenu");
            SoundManager.PlaySFX("MenuSelect");
        }
        else
        {
            connectionModule = new OnlineConnection();
            string response = connectionModule.Connect(username.text, password.text);
            if (response == ConnectedUser.FAIL_TO_CONNECT)
            {
                connectionModule = new OfflineModule();
                response = connectionModule.Connect(username.text, password.text);
            }
            if (response == ConnectedUser.LOGIN_SUCCESFUL)
            {
                Application.LoadLevel("MainMenu");
                SoundManager.PlaySFX("Login");
            }
            if (response == ConnectedUser.FAIL_TO_LOGIN)
            {
                errorMsg.gameObject.SetActive(true);
                SoundManager.PlaySFX("AccesDenied");
            }
        }

    }

    private void InitialiseArenaList()
    {
        GameParameters.ArenaList = new List<Arena>();
        Arena arenaToAdd = new Arena();
        arenaToAdd.id = 0;
        arenaToAdd.name = "Classic";
        arenaToAdd.description = "Classic map";
        arenaToAdd.imagePath = null;
        GameParameters.ArenaList.Add(arenaToAdd);

        arenaToAdd = new Arena();
        arenaToAdd.id = 1;
        arenaToAdd.name = "JoyfulPlatform";
        arenaToAdd.description = "Platforms are fun";
        arenaToAdd.imagePath = null;
        GameParameters.ArenaList.Add(arenaToAdd);


        arenaToAdd = new Arena();
        arenaToAdd.id = 2;
        arenaToAdd.name = "HigherGoals";
        arenaToAdd.description = "The sky is the limit.";
        arenaToAdd.imagePath = null;
        GameParameters.ArenaList.Add(arenaToAdd);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.isFocused)
            {
                password.Select();
            }
            else
            {
                username.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            LogIn();

        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
