using UnityEngine;
using System.Collections;

public class VisualEffects : MonoBehaviour 
{
    public GameObject thunderBoltVisual;

    public void SetThunderBolt()
    {
        thunderBoltVisual.transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
    }

    public void RemoveThunderBolt()
    {
        thunderBoltVisual.transform.position = new Vector3(100, 100, 100);
    }

}