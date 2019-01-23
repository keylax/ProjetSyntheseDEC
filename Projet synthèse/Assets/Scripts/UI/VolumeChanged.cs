using UnityEngine;
using UnityEngine.UI;

public class VolumeChanged : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("Canvas/Panel/GeneralSoundVolume").GetComponent<Slider>().value = GameParameters.MasterVolume;
        GameObject.Find("Canvas/Panel/SoundEffectsVolume").GetComponent<Slider>().value = GameParameters.SoundEffectsVolume;
        GameObject.Find("Canvas/Panel/MusicVolume").GetComponent<Slider>().value = GameParameters.MusicVolume;
    }

    public void ChangeMasterVolume(float _newValue)
    {
        GameParameters.VolumeHasBeenChanged = true;
        GameParameters.MasterVolume = _newValue;
        SoundManager.SetVolume(_newValue);
    }

    public void ChangeSoundEffectsVolume(float _newValue)
    {
        GameParameters.VolumeHasBeenChanged = true;
        GameParameters.SoundEffectsVolume = _newValue;
        SoundManager.SetVolumeSFX(_newValue);
    }

    public void ChangeMusicVolume(float _newValue)
    {
        GameParameters.VolumeHasBeenChanged = true;
        GameParameters.MusicVolume = _newValue;
        SoundManager.SetVolumeMusic(_newValue);
    }

}