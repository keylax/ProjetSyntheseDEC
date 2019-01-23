using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public void Shake()
    {
        GetComponent<Animation>().Play();
    }
}

