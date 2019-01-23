using UnityEngine;
using UnityEngine.UI;

public class PowerListDropDown : MonoBehaviour
{
    private const string MR_FREEZE = "Prevents all opponents from changing their polarity and from activating or de-activating their magnets.";
    private const string SPRING = "Places an object which  makes any player it touches jump up.";
    private const string STAR = "Makes you immune to everything but bottomless pits for a limited duration.";
    private const string MUSHROOM = "Gives you a speed boost.";
    private const string HEAT_SEEKING_MISSILE = "Like a regular missile, except it redirects it self slightly towards the target.";
    private const string LIGHTNING = "Stuns all opponents.";
    private const string MAGNET_OVERWHELMING = "Makes the traction and repulsion force of your magnet much stronger.";
    private const string POLARITY_INVERSION = "Reverses the polarity of the magnets on every opponent's vehicule.";
    private const string MISSILE = "Shoots a missile that explodes on contact, stunning everyone in a certain radius.";

    private Dropdown overseenDropdown;
    private Text affectedText;
    private RawImage affectedImage;

	void Start ()
	{
	    overseenDropdown = GetComponent<Dropdown>();
	    affectedText = transform.parent.GetComponentInChildren<Text>();
        affectedImage = transform.parent.GetComponentInChildren<RawImage>();
	    overseenDropdown.value = 0;
	}
	
    public void HandleMenuClick()
    {
        switch (overseenDropdown.value)
        {
            case 0:
                affectedText.text = MR_FREEZE;
                affectedImage.texture = Resources.Load("BonusesImage/FreezeBonus") as Texture;
                break;

            case 1:
                affectedText.text = SPRING;
                affectedImage.texture = Resources.Load("BonusesImage/SpringBonus") as Texture;
                break;

            case 2:
                affectedText.text = STAR;
                affectedImage.texture = Resources.Load("BonusesImage/StarBonus") as Texture;
                break;

            case 3:
                affectedText.text = MUSHROOM;
                affectedImage.texture = Resources.Load("BonusesImage/BoostBonus") as Texture;
                break;

            case 4:
                affectedText.text = HEAT_SEEKING_MISSILE;
                affectedImage.texture = Resources.Load("BonusesImage/HomingBonus") as Texture;
                break;

            case 5:
                affectedText.text = LIGHTNING;
                affectedImage.texture = Resources.Load("BonusesImage/LightningBonus") as Texture;
                break;

            case 6:
                affectedText.text = MAGNET_OVERWHELMING;
                affectedImage.texture = Resources.Load("BonusesImage/MagnetUpgradeBonus") as Texture;
                break;

            case 7:
                affectedText.text = POLARITY_INVERSION;
                affectedImage.texture = Resources.Load("BonusesImage/ReverseBonus") as Texture;
                break;

            default:
                affectedText.text = MISSILE;
                affectedImage.texture = Resources.Load("BonusesImage/MissileBonus") as Texture;
                break;
        }
    }

}