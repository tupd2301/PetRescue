using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera mainCamera;
    public void UpdateCamera(Vector2 coordinates)
    {
        float scaleX = coordinates.x;
        float scaleY = coordinates.y;

        mainCamera.orthographicSize = Mathf.Max(Mathf.Max(scaleX, scaleY)*2.5f, 15);
    }
}
