using Assets.Scripts.GameController;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public void CustomGame()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("VehicleSelection");
    }

    public void QuickMatch()
    {
        GameParameters.numberOfPlayers = 1;
        GameParameters.playersSelectedModelIndex = new int[1];
        GameParameters.playersSelectedModelIndex[0] = Random.Range(0, 3);
        GameParameters.ChosenArena = GameParameters.ArenaList[Random.Range(0, 3)].name.Replace("\"", "");
        CurrentGame.gameType = CurrentGame.GameType.QUICKMATCH;
        SoundManager.PlaySFX("GameStart");
        Application.LoadLevel("LoadingScene");
    }
}