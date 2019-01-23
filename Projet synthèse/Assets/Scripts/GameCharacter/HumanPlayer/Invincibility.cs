using UnityEngine;

public class Invincibility : MonoBehaviour
{
    public bool isInvincible;

    private const float invicibilityTimerInSeconds = 6;
    private float timeLeftToInvincibility = 0;
	
	public void Update () 
    {
        if (timeLeftToInvincibility > 0)
        {
            timeLeftToInvincibility -= Time.deltaTime;
        }
        else
        {
            isInvincible = false;
        }
	}

    public void MakeInvincible()
    {
        isInvincible = true;
        timeLeftToInvincibility = invicibilityTimerInSeconds;
    }

}