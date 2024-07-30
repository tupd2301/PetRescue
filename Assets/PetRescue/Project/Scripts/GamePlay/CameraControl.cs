using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera mainCamera;
    public void UpdateCamera(Vector2 coordinates)
    {
        float scale = Mathf.Max(Screen.height, Screen.width)/Mathf.Min(Screen.height, Screen.width);
        float scaleX = coordinates.x;
        float scaleY = coordinates.y;

        mainCamera.orthographicSize = Mathf.Max(Mathf.Max(scaleX * scale, scaleY * scale) * 2f, 15);
    }
}
