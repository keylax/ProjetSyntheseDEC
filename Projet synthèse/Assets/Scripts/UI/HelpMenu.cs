using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    public void HowToPlay()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("HowToPlay");
    }

    public void PowerUpsList()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("PowerUpsList");
    }

    public void ControllerControls()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("ControllerControls");
    }

    public void KeyboardControls()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("KeyboardControls");
    }

}