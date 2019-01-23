using UnityEngine;
using System.Collections;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public Button resume;
    public Button quit;

    private bool isPaused;

    void Update()
    {
        if (InputManager.GetPlayerStart(ObjectTags.Player1Tag) && GamestatesController.currentState != GamestatesController.GameStates.GAMEOVER)
        {
            if (isPaused == false)
            {
                Time.timeScale = 0;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                isPaused = true;
                quit.Select();
                resume.Select();
            }
            else if (isPaused == true)
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        isPaused = false;
    }

    public void BackToMenu()
    {
        GamestatesController.currentState = GamestatesController.GameStates.IN_MENUS;
        Time.timeScale = 1;
        Application.LoadLevel("MainMenu");
    }
}
