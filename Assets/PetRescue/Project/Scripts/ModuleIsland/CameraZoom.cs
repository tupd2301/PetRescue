using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraControl))]
public class CameraZoom : MonoBehaviour
{
    private CameraControl cameraControl;

    void Start()
    {
        cameraControl = GetComponent<CameraControl>();
    }

    void Update()
    {
        Vector3 zoomVector = new Vector3(0, -0.1f, 0.1f);
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            if (touch0.phase != TouchPhase.Ended)
            {
                Debug.Log("Zoom");
                if (touch0.deltaPosition.y > 0 || touch1.deltaPosition.y < 0)
                {
                    Debug.Log("ZoomIn");
                    cameraControl.cameraPosition.localPosition += zoomVector;
                }
                if (touch0.deltaPosition.y < 0 || touch1.deltaPosition.y > 0)
                {
                    Debug.Log("ZoomOut");
                    cameraControl.cameraPosition.localPosition -= zoomVector;
                }
            }
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            cameraControl.cameraPosition.localPosition += zoomVector;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            cameraControl.cameraPosition.localPosition -= zoomVector;
        }
        LimitCameraPosition();
    }

    private void LimitCameraPosition()
    {
        float posX = cameraControl.cameraPosition.localPosition.x;
        float posY = cameraControl.cameraPosition.localPosition.y;
        float posZ = cameraControl.cameraPosition.localPosition.z;
        float posYLimitTop = 30f;
        float posZLimitTop = 30f;
        float posYLimitBottom = 10f;
        float posZLimitBottom = 10f;
        float posYNew = Mathf.Min(posYLimitTop, Mathf.Abs(posY)) * (posY > 0 ? 1 : -1);
        float posZNew = Mathf.Min(posZLimitTop, Mathf.Abs(posZ)) * (posZ > 0 ? 1 : -1);
        posYNew = Mathf.Max(posYLimitBottom, Mathf.Abs(posYNew)) * (posY > 0 ? 1 : -1);
        posZNew = Mathf.Max(posZLimitBottom, Mathf.Abs(posZNew)) * (posZ > 0 ? 1 : -1);
        cameraControl.cameraPosition.localPosition = new Vector3(posX, posYNew, posZNew);
    }
}
