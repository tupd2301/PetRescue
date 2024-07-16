using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanArea : MonoBehaviour
{
    public Vector2 PanDir;
    
    public void StartPanning()
    {
        CameraController cameraController = GameManager.GetCameraController();
        if(cameraController)
        {
            cameraController.SetPanDirection(PanDir);
        }
    }

    public void StopPanning()
    {
        CameraController cameraController = GameManager.GetCameraController();
        if (cameraController)
        {
            cameraController.StopPanning();
        }
    }
}
