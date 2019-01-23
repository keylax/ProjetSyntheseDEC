using System;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using UnityEngine;

public class GameObjectFollowerCamera : MonoBehaviour
{

    [System.Serializable]
    //Contains the camera's settings.
    public class CameraPreset
    {
        public float DistanceRelativeToTarget;
        public float HeightRelativeToTarget;
        public float FieldOfView;
    }

    public enum CameraMode
    {
        CLOSE_THIRD_PERSON, FIRST_PERSON, FAR_THIRD_PERSON
    }

    [Header("The camera object to use")]
    public new Camera camera;

    [Header("Wanted camera mode")]
    public CameraMode cameraMode;

    [Header("The player's cannon")]
    public Transform playerCannon;

    public Transform aimObject;

    [Header("The target we are following")]
    public Transform target;

    [Header("Selected Camera Preset")]
    [Tooltip("Active Preset")]
    public CameraPreset selectedCameraPreset;

    [Header("Camera Presets")]
    public CameraPreset firstPersonCameraPreset = new CameraPreset()
    {
        DistanceRelativeToTarget = 0f,
        HeightRelativeToTarget = 0.5f,
        FieldOfView = 40
    };

    public CameraPreset closeThirdPersonCameraPreset = new CameraPreset()
    {
        DistanceRelativeToTarget = 5f,
        HeightRelativeToTarget = 1f,
        FieldOfView = 40
    };

    public CameraPreset farThirdPersonCameraPreset = new CameraPreset()
    {
        DistanceRelativeToTarget = 9f,
        HeightRelativeToTarget = 1f,
        FieldOfView = 40
    };

    private Vector3 cameraPosition;
    private float rotationY;
    private Quaternion lookUpQuaternion;
    private float lastHorizontalAxis;
    private bool isJoystickUp;
    private bool isJoystickDead = true;
    private const float aimObjectDefaultHeight = 0.7f;
    private const float aimObjectMaxHeight = 3f;
    private LayerMask ballIndicatorLayerMask;
    private LayerMask firstPersonLayerMask;

    void Start()
    {
        if (target == null)
        {
            enabled = false;
            throw new Exception("GameObjectFollowerCamera script on \"" + name + "\" must have a GameObject to follow. Please set one in the inspector.");
        }
        if (target.GetComponent<Rigidbody>() == null)
        {
            enabled = false;
            throw new Exception("GameObjectFollowerCamera script on \"" + name + "\" must have a GameObject with a RigidBody. Please add a RigidBody to the GameObject.");
        }

        SetCameraMode(cameraMode);

        camera.fieldOfView = selectedCameraPreset.FieldOfView;
    }


    void LateUpdate()
    {
        // Calculate the current rotation angles
        float currentRotationAngle = transform.eulerAngles.y;

        // Convert the angle into a rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * selectedCameraPreset.DistanceRelativeToTarget;

        //Compensate for camera collisions.
        cameraPosition = transform.position + new Vector3(0f, selectedCameraPreset.HeightRelativeToTarget, 0f);

        if (cameraMode != CameraMode.FIRST_PERSON)
        {
            CompensateForCollision(target.transform.position, ref cameraPosition);
        }

        transform.position = new Vector3(cameraPosition.x, target.transform.position.y + selectedCameraPreset.HeightRelativeToTarget, cameraPosition.z);

        if (IsGamepadConnected())
        {
            //Use the Y axis of the controller if it is connected.
            rotationY += InputManager.GetPlayerAimAxis(transform.parent.gameObject.tag) * Time.deltaTime;
        }

        CheckJoystickStatus();
        lastHorizontalAxis = rotationY;

        MoveAimObject();

        MakeMagnetAndCannonLookAtAimObject();
        transform.LookAt(aimObject);
   
    }

    private void MoveAimObject()
    {
        if (isJoystickUp)
        {
            if (!isJoystickDead)
            {
                if (aimObject.localPosition.y + 0.15f <= aimObjectMaxHeight)
                {
                    aimObject.localPosition = new Vector3(aimObject.localPosition.x, aimObject.localPosition.y + 0.15f,
                        aimObject.localPosition.z);

                }
                else
                {
                    aimObject.localPosition =  new Vector3(aimObject.localPosition.x, aimObjectMaxHeight, aimObject.localPosition.z);
                }
            }
        }
        else if (!isJoystickUp)
        {
            if (!isJoystickDead)
            {
                if (aimObject.localPosition.y - 0.15f >= aimObjectDefaultHeight)
                {
                    aimObject.localPosition = new Vector3(aimObject.localPosition.x, aimObject.localPosition.y - 0.15f,
                        aimObject.localPosition.z);
                }
                else
                {
                    aimObject.localPosition = new Vector3(aimObject.localPosition.x, aimObjectDefaultHeight, aimObject.localPosition.z);

                }
            }
        }
    }

    private void MakeMagnetAndCannonLookAtAimObject()
    {
        if (aimObject.localPosition.y > aimObjectMaxHeight)
        {
            playerCannon.LookAt(new Vector3(aimObject.localPosition.x, 2, aimObject.localPosition.z));
            transform.parent.GetChild(4).LookAt(new Vector3(aimObject.localPosition.x, 2, aimObject.localPosition.z));
        }
        else if (aimObject.localPosition.y < aimObjectDefaultHeight)
        {
            playerCannon.LookAt(new Vector3(aimObject.localPosition.x, 0.7f, aimObject.localPosition.z));
            transform.parent.GetChild(4).LookAt(new Vector3(aimObject.localPosition.x, 0.7f, aimObject.localPosition.z));
        }
        else
        {
            playerCannon.LookAt(aimObject);
            transform.parent.GetChild(4).LookAt(aimObject);
        }
    }

    private void CheckJoystickStatus()
    {
        if (lastHorizontalAxis < rotationY)
        {
            isJoystickUp = true;
            isJoystickDead = false;
        }
        else if (lastHorizontalAxis > rotationY)
        {
            isJoystickUp = false;
            isJoystickDead = false;
        }
        else
        {
            isJoystickDead = true;
        }
    }


    private void HideOtherPlayersBallIndicators()
    {
        switch (transform.parent.tag)
        {
            case ObjectTags.Player1Tag:
                ballIndicatorLayerMask = 1 << LayerMask.NameToLayer("Player2BallIndicator") | 1 << LayerMask.NameToLayer("Player3BallIndicator") | 1 << LayerMask.NameToLayer("Player4BallIndicator");
                break;

            case ObjectTags.Player2Tag:
                ballIndicatorLayerMask = 1 << LayerMask.NameToLayer("Player1BallIndicator") | 1 << LayerMask.NameToLayer("Player3BallIndicator") | 1 << LayerMask.NameToLayer("Player4BallIndicator");
                break;

            case ObjectTags.Player3Tag:
                ballIndicatorLayerMask = 1 << LayerMask.NameToLayer("Player1BallIndicator") | 1 << LayerMask.NameToLayer("Player2BallIndicator") | 1 << LayerMask.NameToLayer("Player4BallIndicator");
                break;

            case ObjectTags.Player4Tag:
                ballIndicatorLayerMask = 1 << LayerMask.NameToLayer("Player1BallIndicator") | 1 << LayerMask.NameToLayer("Player2BallIndicator") | 1 << LayerMask.NameToLayer("Player3BallIndicator");
                break;
        }
        camera.cullingMask = ~ballIndicatorLayerMask;
    }

    private void HidePlayerFromHisOwnCamera()
    {
        switch (transform.parent.tag)
        {
            case ObjectTags.Player1Tag:
                firstPersonLayerMask = 1 << LayerMask.NameToLayer("Player1") | 1 << LayerMask.NameToLayer("Player2BallIndicator") | 1 << LayerMask.NameToLayer("Player3BallIndicator") | 1 << LayerMask.NameToLayer("Player4BallIndicator");
                break;

            case ObjectTags.Player2Tag:
                firstPersonLayerMask = 1 << LayerMask.NameToLayer("Player2") | 1 << LayerMask.NameToLayer("Player1BallIndicator") | 1 << LayerMask.NameToLayer("Player3BallIndicator") | 1 << LayerMask.NameToLayer("Player4BallIndicator");
                break;

            case ObjectTags.Player3Tag:
                firstPersonLayerMask = 1 << LayerMask.NameToLayer("Player3") | 1 << LayerMask.NameToLayer("Player1BallIndicator") | 1 << LayerMask.NameToLayer("Player2BallIndicator") | 1 << LayerMask.NameToLayer("Player4BallIndicator");
                break;

            case ObjectTags.Player4Tag:
                firstPersonLayerMask = 1 << LayerMask.NameToLayer("Player4") | 1 << LayerMask.NameToLayer("Player1BallIndicator") | 1 << LayerMask.NameToLayer("Player2BallIndicator") | 1 << LayerMask.NameToLayer("Player3BallIndicator");
                break;
        }
        camera.cullingMask = ~firstPersonLayerMask;
    }

    public void SetCameraMode(CameraMode _cameraMode)
    {
        switch (_cameraMode)
        {
            case CameraMode.FIRST_PERSON:
                selectedCameraPreset = firstPersonCameraPreset;
                HidePlayerFromHisOwnCamera();
                break;
            case CameraMode.CLOSE_THIRD_PERSON:
                selectedCameraPreset = closeThirdPersonCameraPreset;
                HideOtherPlayersBallIndicators();
                break;
            case CameraMode.FAR_THIRD_PERSON:
                selectedCameraPreset = farThirdPersonCameraPreset;
                HideOtherPlayersBallIndicators();
                break;
        }
    }


    private bool IsGamepadConnected()
    {
        String[] joysticks = new string[Input.GetJoystickNames().Length];

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            joysticks[i] = Input.GetJoystickNames()[i];
        }

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (joysticks[i].Contains("Controller"))
            {
                //A controller is connected
                return true;
            }
        }

        //There is no controller connected
        return false;
    }

    private void CompensateForCollision(Vector3 _fromObject, ref Vector3 _toTarget)
    {
        Debug.DrawLine(_fromObject, _toTarget, Color.cyan);

        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(_fromObject, _toTarget, out wallHit, layerMask))
        {
            Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
            Vector3 distanceVec = wallHit.point - _toTarget;
            _toTarget = new Vector3(wallHit.point.x, wallHit.point.y, wallHit.point.z);
            _toTarget += distanceVec.normalized * 0.25f;
        }
    }

}