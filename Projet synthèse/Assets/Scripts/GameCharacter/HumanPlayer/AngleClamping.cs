using UnityEngine;
using System.Collections;

public class AngleClamping : MonoBehaviour
{
    private Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        float xAngle = playerRigidbody.transform.eulerAngles.x;

        xAngle += 180;

        if (xAngle > 360)
        {
            xAngle -= 360;
        }

        xAngle = Mathf.Clamp(xAngle, 140, 200);

        xAngle -= 180;
        playerRigidbody.transform.eulerAngles = new Vector3(xAngle, transform.eulerAngles.y, 0);
    }
}
