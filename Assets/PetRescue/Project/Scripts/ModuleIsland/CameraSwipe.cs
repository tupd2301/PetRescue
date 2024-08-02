using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraControl))]
public class CameraSwipe : MonoBehaviour
{
    private CameraControl cameraControl;

    void Start()
    {
        cameraControl = GetComponent<CameraControl>();
    }

    void Update()
    {
        Vector3 swipeVector = new Vector3(0, 1, 0);
        if (Input.touchCount == 2)
            return;
        if (Input.touchCount == 1)
        {
            Touch touch0 = Input.GetTouch(0);
            if (touch0.phase == TouchPhase.Moved)
            {
                Debug.Log("Swipe");
                if (touch0.deltaPosition.x > 0)
                {
                    Debug.Log("SwipeRight");
                    cameraControl.cameraRotation.localEulerAngles += swipeVector;
                }
                if (touch0.deltaPosition.x < 0)
                {
                    Debug.Log("SwipeLeft");
                    cameraControl.cameraRotation.localEulerAngles -= swipeVector;
                }
            }
        }

        if (Input.GetMouseButton(1) && Input.GetAxis("Mouse X") != 0)
        {
            // cameraControl.cameraPosition.localPosition +=
            //     swipeVector * (Input.GetAxis("Mouse X") > 0f ? 1f : -1f);
            cameraControl.cameraRotation.localEulerAngles = Vector3.Lerp(
                cameraControl.cameraRotation.localEulerAngles,
                cameraControl.cameraRotation.localEulerAngles
                    + swipeVector * (Input.GetAxis("Mouse X") > 0f ? 1f : -1f),
                Time.deltaTime * 100
            );
        }
    }
}
