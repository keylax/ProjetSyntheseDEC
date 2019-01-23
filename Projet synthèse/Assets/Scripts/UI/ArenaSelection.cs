using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.AchievementModule;
using Assets.Scripts.ConnectionModule;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using UnityEngine.UI;

public class ArenaSelection : MonoBehaviour
{
    [Header("The text object that contains the arena's name.")]
    public Text arenaName;
    [Header("The text object that contains the arena's epic description.")]
    public Text description;
    [Header("The image object that contains the arena's image.")]
    public Image arenaImage;
    [Header("The image object that indicates if an arena is locked (big red X).")]
    public Image lockedArenaImage;
    [Header("The error message to display when the arena selected is locked.")]
    public Text errorMsg;


    private bool isPlayer1AxisInUse;
    private IList<Arena> arenaList;
    private int selectedArenaIndex = 0;
    private WWW www;
    private string formattedName;
    private string formattedDescription;
    private const string SERVER_URL = "http://duckhunters.webuda.com";

    void Start()
    {
        arenaList = GameParameters.ArenaList;
        SetPicture();
    }

    void Update()
    {
        if (InputManager.GetPlayerHorizontal(ObjectTags.Player1Tag) > 0.75f)
        {
            if (isPlayer1AxisInUse == false)
            {
                isPlayer1AxisInUse = true;
                ChangeSelectedArena(true);
            }
        }
        else if (InputManager.GetPlayerHorizontal(ObjectTags.Player1Tag) < -0.75f)
        {
            if (isPlayer1AxisInUse == false)
            {
                isPlayer1AxisInUse = true;
                ChangeSelectedArena(false);
            }
        }
        else if (InputManager.GetPlayerHorizontal(ObjectTags.Player1Tag) == 0)
        {
            isPlayer1AxisInUse = false;
        }

        if (InputManager.GetPlayerStart(ObjectTags.Player1Tag))
        {
            if (!IsArenaLocked(selectedArenaIndex))
            {
                GameParameters.ChosenArena = arenaList[selectedArenaIndex].name.Replace("\"", "");
                CurrentGame.gameType = CurrentGame.GameType.CUSTOMGAME;
                SoundManager.PlaySFX("GameStart");
                Application.LoadLevel("LoadingScene");
            }
            else
            {
                errorMsg.gameObject.SetActive(true);
            }
        }

        SetInterfaceElements();
    }

    private void SetInterfaceElements()
    {
        formattedName = arenaList[selectedArenaIndex].name.Replace("\"", "");
        arenaName.text = formattedName;

        formattedDescription = arenaList[selectedArenaIndex].description.Replace("\"", "");
        description.text = formattedDescription;

        if (www != null && www.isDone)
        {
            Material material = new Material(arenaImage.material);
            material.mainTexture = www.texture;
            arenaImage.material = material;
            www = null;
        }
    }

    public void ChangeSelectedArena(bool _previousOrNext)
    {
        if (_previousOrNext)
        {
            if (selectedArenaIndex == arenaList.Count - 1)
            {
                selectedArenaIndex = 0;
                IsArenaLocked(selectedArenaIndex);
            }
            else
            {
                selectedArenaIndex++;
                IsArenaLocked(selectedArenaIndex);
            }
        }
        else
        {
            if (selectedArenaIndex == 0)
            {
                selectedArenaIndex = arenaList.Count - 1;
                IsArenaLocked(selectedArenaIndex);
            }
            else
            {
                selectedArenaIndex--;
                IsArenaLocked(selectedArenaIndex);
            }
        }

        SetPicture();
    }

    private void SetPicture()
    {
        if (ConnectedUser.connectedUser != null && ConnectedUser.connectedUser.isOnline)
        {
            string formattedImageUrl = arenaList[selectedArenaIndex].imagePath;
            formattedImageUrl = formattedImageUrl.Replace("\"", "");
            formattedImageUrl = formattedImageUrl.Replace("\\", "");
            www = new WWW(SERVER_URL + "/" + formattedImageUrl);
        }
    }

    private bool IsArenaLocked(int _selectedArenaIndex)
    {
        bool isLocked = false;
        if (ConnectedUser.connectedUser != null)
        {
            switch (_selectedArenaIndex)
            {
                case 1:
                    if (Achievement.GetProgressById(Achievement.GetIdByName("OhBabyATriple")) != 100)
                    {
                        isLocked = true;
                    }
                    break;

                case 2:
                    if (Achievement.GetProgressById(Achievement.GetIdByName("TotalDomination")) != 100)
                    {
                        isLocked = true;
                    }
                    break;
            }
        }
        lockedArenaImage.gameObject.SetActive(isLocked);
        return isLocked;
    }
}
