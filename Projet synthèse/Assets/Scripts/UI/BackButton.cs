using System;
using System.IO;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public string GoToScene;

    public void OnButtonPress()
    {
        if (Application.loadedLevelName == "AudioMenu")
        {
            if (GameParameters.VolumeHasBeenChanged)
            {
                try
                {
                    StreamWriter soundSettingsWriter = new StreamWriter(Application.dataPath + "/soundConfig.txt", false);

                    soundSettingsWriter.WriteLine(GameParameters.MasterVolume.ToString());
                    soundSettingsWriter.WriteLine(GameParameters.MusicVolume.ToString());
                    soundSettingsWriter.WriteLine(GameParameters.SoundEffectsVolume.ToString());
                    soundSettingsWriter.WriteLine(GameParameters.AnnouncerVolume.ToString());

                    soundSettingsWriter.Close();
                }
                catch (Exception)
                {
                    Debug.Log("Failed to save sound settings.");
                }
            }
        }

        Application.LoadLevel(GoToScene);
        SoundManager.PlaySFX("BackMenu");
    }

    void Update()
    {
        if (InputManager.GetPlayerBack(ObjectTags.Player1Tag))
        {
            OnButtonPress();
        }
    }

}