using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlsUIElement : MonoBehaviour
{
    public void ZoomIn()
    {
        CameraController camController = GameManager.GetCameraController();
        if(camController)
        {
            camController.ZoomIn();
        }
    }

    public void ZoomOut()
    {
        CameraController camController = GameManager.GetCameraController();
        if (camController)
        {
            camController.ZoomOut();
        }
    }

    public void RotateRight()
    {
        CameraController camController = GameManager.GetCameraController();
        if (camController)
        {
            camController.RotateRight();
        }
    }

    public void RotateLeft()
    {
        CameraController camController = GameManager.GetCameraController();
        if (camController)
        {
            camController.RotateLeft();
        }
    }
}
