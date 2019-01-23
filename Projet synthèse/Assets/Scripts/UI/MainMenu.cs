using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text username;
    public Image avatar;
    public Button profileButton;
    public Button logoutButton;
    private WWW www;

    private const string SERVER_URL = "http://duckhunters.webuda.com";

    void Start()
    {

        if (ConnectedUser.connectedUser != null)
        {
            username.text = ConnectedUser.connectedUser.name;
            if (ConnectedUser.connectedUser.avatarPath != null)
            {
                string formattedImageUrl = ConnectedUser.connectedUser.avatarPath;
                formattedImageUrl = formattedImageUrl.Replace("\"", "");
                formattedImageUrl = formattedImageUrl.Replace("\\", "");
                www = new WWW(SERVER_URL + "/" + formattedImageUrl);
                username.text = ConnectedUser.connectedUser.name.Replace("\"", "");
            }

        }
        else
        {
            username.text = "Guest";
            profileButton.gameObject.SetActive(false);
            logoutButton.GetComponentInChildren<Text>().text = "Login";
        }
    }

    void Update()
    {
        if (www != null && www.isDone)
        {
            Material material = new Material(avatar.material);
            material.mainTexture = www.texture;
            avatar.material = material;
            www = null;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LogOut()
    {
        ConnectedUser.connectedUser = null;
        SoundManager.PlaySFX("BackMenu");
        Application.LoadLevel("LogInMenu");
    }

    public void Play()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("GameMenu");
    }

    public void Options()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("AudioMenu");
    }

    public void Help()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("HelpMenu");
    }

    public void Credits()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("Credits");
    }

    public void Profile()
    {
        SoundManager.PlaySFX("MenuSelect");
        Application.LoadLevel("PlayerProfile");
    }

}