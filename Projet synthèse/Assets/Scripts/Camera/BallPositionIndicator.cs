using System;
using UnityEngine;


public class BallPositionIndicator : MonoBehaviour
{

    [Header("Point to:")]
    public Transform ball;

    [Header("In Camera:")]
    public GameObject cameraContainer;

    private Vector3 localOffset = new Vector3(0, 2.25f, 8f);
    private new Camera camera;
    private Renderer ballIndicatorRenderer;

    void Start()
    {
        ballIndicatorRenderer = transform.GetComponentInChildren<Renderer>();

        if (cameraContainer.GetComponent<GameObjectFollowerCamera>().camera != null)
        {
            camera = cameraContainer.GetComponent<GameObjectFollowerCamera>().camera;
        }
        else
        {
            enabled = false;
            throw new Exception("Could not find camera in camera container");
        }
    }


    void LateUpdate()
    {
        ballIndicatorRenderer.enabled = !isVisible();


        Vector3 worldOffset = camera.transform.rotation * localOffset;
        Vector3 spawnPosition = camera.transform.position + worldOffset;

        transform.position = spawnPosition;

        transform.LookAt(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));

    }

    private bool isVisible()
    {
        Vector3 ballPos = ball.position - camera.transform.position;
        float angle = Vector3.Angle(ballPos, camera.transform.forward);

        return angle < camera.fieldOfView;

    }
}
