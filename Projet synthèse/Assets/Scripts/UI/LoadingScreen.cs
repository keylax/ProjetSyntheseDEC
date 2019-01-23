using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private string DEFAULT_ARENA = "Classic";
    private const int NUMBER_OF_ARRAYS = 9;
    private readonly string[] TIPS =
    {
        "Remember kids, don't drink and drive!",
        "Tip: Sometimes, tips are displayed during loading screens.",
        "To win a game, play better than your opponent. Or be luckier... either works.",
        "Don't be a game developper, sleep for hours everyday!",
        "Throwing yourself in a bottomless pit is not the optimal strategy...",
        "The best use for a console is obviously as a screen stand.",
        "Quick! Use this loading time to poke someone on facebook! Oh too late...",
        "Oh look! A dragon! Oh too late it's gone...",
        "They see me loadin' ... They hatin' ..."
    };

    public Texture2D emptyProgressBar;
    public Texture2D fullProgressBar;

    private AsyncOperation async;

    void Start()
    {
        int randomTip = Random.Range(0, NUMBER_OF_ARRAYS);
        GetComponentInChildren<Text>().text = TIPS[randomTip];

        if (!string.IsNullOrEmpty(GameParameters.ChosenArena))
        {
            StartCoroutine(LoadALevel(GameParameters.ChosenArena));
        }
        else
        {
            StartCoroutine(LoadALevel(DEFAULT_ARENA));
        }
    }

    private IEnumerator LoadALevel(string _levelName)
    {
        async = Application.LoadLevelAsync(_levelName);
        async.allowSceneActivation = false;
        yield return async;
    }

    private void GoToScene()
    {
        async.allowSceneActivation = true;
    }

    void Update()
    {
        if (InputManager.GetPlayerStart(ObjectTags.Player1Tag))
        {
            GoToScene();
        }
        if (InputManager.GetPlayerStart(ObjectTags.Player2Tag))
        {
            GoToScene();
        }
        if (InputManager.GetPlayerStart(ObjectTags.Player3Tag))
        {
            GoToScene();
        }
        if (InputManager.GetPlayerStart(ObjectTags.Player4Tag))
        {
            GoToScene();
        }
    }

    private void OnGUI()
    {
        if (async != null)
        {
            //The +0.1f accounts for the Unity misktake of loading "never" finishing and stopping at 0.9f...
            //(this appears to be a bug in Unity)

            float actualCompletionPercentage = (async.progress + 0.1f);

            Graphics.DrawTexture(new Rect(Screen.width / 3, Screen.height - Screen.height / 6, Screen.width / 3, Screen.height / 15), emptyProgressBar);
            Graphics.DrawTexture(new Rect(Screen.width / 3, Screen.height - Screen.height / 6, Screen.width / 3 * actualCompletionPercentage, Screen.height / 15), fullProgressBar);

            if (actualCompletionPercentage >= 1f)
            {
                GetComponentsInChildren<Text>().ElementAt(1).text = "Press start to... well start...";
            }
            else
            {
                GetComponentsInChildren<Text>().ElementAt(1).text = (actualCompletionPercentage * 100).ToString() + "%";
            }
        }
    }

}