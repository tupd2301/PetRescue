using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger : MonoBehaviour
{
    public BaseComponent baseComponent;
    private Collider coll;
    void Start()
    {
        coll = GetComponent<Collider>();
    }
    void Update()
    {
        if (Input.touchCount > 0 && baseComponent.isHide == false)
        {
            Touch touch = Input.GetTouch(0);

            // Kiểm tra nếu ngón tay vừa chạm xuống màn hình
            if (touch.phase == TouchPhase.Began)
            {
                TriggerTouch();
            }
        }
        if ((Input.GetMouseButtonDown(0)) && baseComponent.isHide == false)
        {
            TriggerTouch();
        }
    }
    private void TriggerTouch()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (coll.Raycast(ray, out hit, 100))
            {
                Debug.Log("Clicked on Cube");
                baseComponent.OnBaseClicked();
            }
    }
}
