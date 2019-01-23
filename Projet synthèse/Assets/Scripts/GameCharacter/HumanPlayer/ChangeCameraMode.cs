using Assets.Scripts.GameController;
using UnityEngine;

public class ChangeCameraMode : MonoBehaviour
{
	void Update () 
    {
        if (InputManager.GetPlayerChangeCamera(tag))
        {
            GameObjectFollowerCamera cameraScript = transform.GetChild(0).GetComponent<GameObjectFollowerCamera>();
            if (cameraScript.selectedCameraPreset == cameraScript.closeThirdPersonCameraPreset)
            {
                cameraScript.SetCameraMode(GameObjectFollowerCamera.CameraMode.FIRST_PERSON);
            }
            else if (cameraScript.selectedCameraPreset == cameraScript.farThirdPersonCameraPreset)
            {
                cameraScript.SetCameraMode(GameObjectFollowerCamera.CameraMode.CLOSE_THIRD_PERSON);
            }
            else
            {
                cameraScript.SetCameraMode(GameObjectFollowerCamera.CameraMode.FAR_THIRD_PERSON);
            }
        }
	}
}
