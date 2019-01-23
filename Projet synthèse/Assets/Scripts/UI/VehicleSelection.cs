using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.AchievementModule;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using UnityEngine.UI;

public class VehicleSelection : MonoBehaviour
{
    [Header("The list of models for each player.")]
    public List<GameObject> player1ModelList;
    public List<GameObject> player2ModelList;
    public List<GameObject> player3ModelList;
    public List<GameObject> player4ModelList;

    [Header("The 'Press Start' text of each player.")]
    public Text player2PressStartText;
    public Text player3PressStartText;
    public Text player4PressStartText;

    [Header("The list of model locked images (big red X).")]
    public List<Image> playersModelLockedList;

    [Header("The error message to display when one or more vehicle selected are locked.")]
    public Text errorMsg;

    //Used to prevent the cycling through the models from occuring every frame.
    private bool isPlayer1AxisInUse;
    private bool isPlayer2AxisInUse;
    private bool isPlayer3AxisInUse;
    private bool isPlayer4AxisInUse;

    //Determines if a player is considered active.
    private bool isPlayer2Active;
    private bool isPlayer3Active;
    private bool isPlayer4Active;

    private readonly int[] playersSelectedIndex = new int[4];
    private readonly List<List<GameObject>> playersModelLists = new List<List<GameObject>>();
    private int nbrOfPlayers = 1;

    void Start()
    {
        playersModelLists.Add(player1ModelList);
        playersModelLists.Add(player2ModelList);
        playersModelLists.Add(player3ModelList);
        playersModelLists.Add(player4ModelList);
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            CheckPlayersInputs(i);
        }

        if (InputManager.GetPlayerStart(ObjectTags.Player1Tag))
        {
            if (!IsAnyModelSelectedLocked())
            {
                GameParameters.numberOfPlayers = nbrOfPlayers;
                GameParameters.playersSelectedModelIndex = new int[nbrOfPlayers];

                for (int i = 0; i < nbrOfPlayers; i++)
                {
                    GameParameters.playersSelectedModelIndex[i] = playersSelectedIndex[i];
                }

                SoundManager.PlaySFX("MenuSelect");
                Application.LoadLevel("ArenaSelection");
            }
            else
            {
                errorMsg.gameObject.SetActive(true);
            }
        }
    }

    private void CheckPlayersInputs(int _index)
    {
        switch (_index)
        {
            case 0:
                CheckPlayer1Inputs();
                break;

            case 1:
                CheckPlayer2Inputs();
                break;

            case 2:
                CheckPlayer3Inputs();
                break;

            case 3:
                CheckPlayer4Inputs();
                break;
        }
    }

    private void CheckPlayer1Inputs()
    {
        if (InputManager.GetPlayerHorizontal(ObjectTags.Player1Tag) > 0)
        {
            if (isPlayer1AxisInUse == false)
            {
                isPlayer1AxisInUse = true;
                ChangePlayerSelectedModel(true, 0, ref playersSelectedIndex[0]);
            }
        }
        else if (InputManager.GetPlayerHorizontal(ObjectTags.Player1Tag) < 0)
        {
            if (isPlayer1AxisInUse == false)
            {
                isPlayer1AxisInUse = true;
                ChangePlayerSelectedModel(false, 0, ref playersSelectedIndex[0]);
            }
        }
        else if (InputManager.GetPlayerHorizontal(ObjectTags.Player1Tag) == 0)
        {
            isPlayer1AxisInUse = false;
        }
    }

    private void CheckPlayer2Inputs()
    {
        if (InputManager.GetPlayerBack(ObjectTags.Player2Tag))
        {
            if (isPlayer2Active)
            {
                player2PressStartText.gameObject.SetActive(true);
                nbrOfPlayers--;
                isPlayer2Active = false;
                player2ModelList[playersSelectedIndex[1]].gameObject.SetActive(false);
                playersSelectedIndex[1] = 0;
            }
        }

        if (InputManager.GetPlayerStart(ObjectTags.Player2Tag))
        {
            if (!isPlayer2Active)
            {
                player2PressStartText.gameObject.SetActive(false);
                nbrOfPlayers++;
                isPlayer2Active = true;
                player2ModelList[0].gameObject.SetActive(true);
            }
        }

        if (isPlayer2Active)
        {
            if (InputManager.GetPlayerHorizontal(ObjectTags.Player2Tag) > 0.75f)
            {
                if (isPlayer2AxisInUse == false)
                {
                    isPlayer2AxisInUse = true;
                    ChangePlayerSelectedModel(true, 1, ref playersSelectedIndex[1]);
                }
            }
            else if (InputManager.GetPlayerHorizontal(ObjectTags.Player2Tag) < -0.75f)
            {
                if (isPlayer2AxisInUse == false)
                {
                    isPlayer2AxisInUse = true;
                    ChangePlayerSelectedModel(false, 1, ref playersSelectedIndex[1]);
                }
            }
            else if (InputManager.GetPlayerHorizontal(ObjectTags.Player2Tag) == 0)
            {
                isPlayer2AxisInUse = false;
            }
        }
    }

    private void CheckPlayer3Inputs()
    {
        if (InputManager.GetPlayerBack(ObjectTags.Player3Tag))
        {
            if (isPlayer3Active)
            {
                player3PressStartText.gameObject.SetActive(true);
                nbrOfPlayers--;
                isPlayer3Active = false;
                player2ModelList[playersSelectedIndex[2]].gameObject.SetActive(false);
                playersSelectedIndex[2] = 0;

            }
        }

        if (InputManager.GetPlayerStart(ObjectTags.Player3Tag))
        {
            if (!isPlayer3Active)
            {
                player3PressStartText.gameObject.SetActive(false);
                nbrOfPlayers++;
                isPlayer3Active = true;
                player3ModelList[0].gameObject.SetActive(true);
            }
        }

        if (isPlayer3Active)
        {
            if (InputManager.GetPlayerHorizontal(ObjectTags.Player3Tag) > 0.75f)
            {
                if (isPlayer3AxisInUse == false)
                {
                    isPlayer3AxisInUse = true;
                    ChangePlayerSelectedModel(true, 2, ref playersSelectedIndex[2]);
                }
            }
            else if (InputManager.GetPlayerHorizontal(ObjectTags.Player3Tag) < -0.75f)
            {
                if (isPlayer3AxisInUse == false)
                {
                    isPlayer3AxisInUse = true;
                    ChangePlayerSelectedModel(false, 2, ref playersSelectedIndex[2]);
                }
            }
            else if (InputManager.GetPlayerHorizontal(ObjectTags.Player3Tag) == 0)
            {
                isPlayer3AxisInUse = false;
            }
        }
    }

    private void CheckPlayer4Inputs()
    {
        if (InputManager.GetPlayerBack(ObjectTags.Player4Tag))
        {
            if (isPlayer4Active)
            {
                player4PressStartText.gameObject.SetActive(true);
                nbrOfPlayers--;
                isPlayer4Active = false;
                player2ModelList[playersSelectedIndex[3]].gameObject.SetActive(false);
                playersSelectedIndex[3] = 0;
            }
        }

        if (InputManager.GetPlayerStart(ObjectTags.Player4Tag))
        {
            if (!isPlayer4Active)
            {
                player4PressStartText.gameObject.SetActive(false);
                nbrOfPlayers++;
                isPlayer4Active = true;
                player4ModelList[0].gameObject.SetActive(true);
            }
        }

        if (isPlayer4Active)
        {
            if (InputManager.GetPlayerHorizontal(ObjectTags.Player4Tag) > 0.75f)
            {
                if (isPlayer4AxisInUse == false)
                {
                    isPlayer4AxisInUse = true;
                    ChangePlayerSelectedModel(true, 3, ref playersSelectedIndex[3]);
                }
            }
            else if (InputManager.GetPlayerHorizontal(ObjectTags.Player4Tag) < -0.75f)
            {
                if (isPlayer4AxisInUse == false)
                {
                    isPlayer4AxisInUse = true;
                    ChangePlayerSelectedModel(false, 3, ref playersSelectedIndex[3]);
                }
            }
            else if (InputManager.GetPlayerHorizontal(ObjectTags.Player4Tag) == 0)
            {
                isPlayer4AxisInUse = false;
            }
        }
    }


    public void Player1MouseChangeModel(bool _previousOrNext)
    {
        ChangePlayerSelectedModel(_previousOrNext, 0, ref playersSelectedIndex[0]);
    }

    private void ChangePlayerSelectedModel(bool _previousOrNext, int _playerIndex, ref int _selectedModelIndex)
    {
        if (_previousOrNext)
        {
            if (_selectedModelIndex != playersModelLists[_playerIndex].Count - 1)
            {
                playersModelLists[_playerIndex][_selectedModelIndex].gameObject.SetActive(false);
                _selectedModelIndex++;
                playersModelLists[_playerIndex][_selectedModelIndex].gameObject.SetActive(true);
                if (IsModelLocked(_selectedModelIndex))
                {
                    playersModelLockedList[_playerIndex].gameObject.SetActive(true);
                }
                else
                {
                    playersModelLockedList[_playerIndex].gameObject.SetActive(false);
                }
            }
            else
            {
                playersModelLists[_playerIndex][_selectedModelIndex].gameObject.SetActive(false);
                _selectedModelIndex = 0;
                playersModelLists[_playerIndex][_selectedModelIndex].gameObject.SetActive(true);
                if (IsModelLocked(_selectedModelIndex))
                {
                    playersModelLockedList[_playerIndex].gameObject.SetActive(true);
                }
                else
                {
                    playersModelLockedList[_playerIndex].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (_selectedModelIndex != 0)
            {
                playersModelLists[_playerIndex][_selectedModelIndex].gameObject.SetActive(false);
                _selectedModelIndex--;
                playersModelLists[_playerIndex][_selectedModelIndex].gameObject.SetActive(true);
                if (IsModelLocked(_selectedModelIndex))
                {
                    playersModelLockedList[_playerIndex].gameObject.SetActive(true);
                }
                else
                {
                    playersModelLockedList[_playerIndex].gameObject.SetActive(false);
                }
            }
            else
            {
                playersModelLists[_playerIndex][_selectedModelIndex].gameObject.SetActive(false);
                _selectedModelIndex = playersModelLists[_playerIndex].Count - 1;
                playersModelLists[_playerIndex][_selectedModelIndex].gameObject.SetActive(true);
                if (IsModelLocked(_selectedModelIndex))
                {
                    playersModelLockedList[_playerIndex].gameObject.SetActive(true);
                }
                else
                {
                    playersModelLockedList[_playerIndex].gameObject.SetActive(false);
                }
            }
        }
    }

    private bool IsModelLocked(int _selectedModelIndex)
    {
        bool isLocked = false;
        if (ConnectedUser.connectedUser != null)
        {
            switch (_selectedModelIndex)
            {
                case 1:
                    if (Achievement.GetProgressById(Achievement.GetIdByName("WomboCombo")) != 100)
                    {
                        isLocked = true;
                    }
                    break;

                case 2:
                    if (Achievement.GetProgressById(Achievement.GetIdByName("NewbiePhaseOver")) != 100)
                    {
                        isLocked = true;
                    }
                    break;

                case 3:
                    if (Achievement.GetProgressById(Achievement.GetIdByName("ExplosionManiac")) != 100)
                    {
                        isLocked = true;
                    }
                    break;
            }
        }
        return isLocked;
    }

    private bool IsAnyModelSelectedLocked()
    {
        bool isAnyModelLocked = false;
        for (int i = 0; i < nbrOfPlayers; i++)
        {
            if (IsModelLocked(playersSelectedIndex[i]))
            {
                isAnyModelLocked = true;
            }
        }
        return isAnyModelLocked;
    }

    private void GetFirstUnlockedModel(ref int _selectedModelIndex, int _playerIndex)
    {
        while (IsModelLocked(_selectedModelIndex))
        {
            if (_selectedModelIndex != playersModelLists[_playerIndex].Count - 1)
            {
                _selectedModelIndex++;
            }
            else
            {
                _selectedModelIndex = 0;
            }
        }
    }
}
