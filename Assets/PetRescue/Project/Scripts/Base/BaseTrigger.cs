using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger : MonoBehaviour
{
    public BaseComponent baseComponent;
    private Collider coll;
    private int currentFingerId;

    public System.Action onBaseClickedForTutorial;
    void Start()
    {
        coll = GetComponent<Collider>();
    }
    void Update()
    {
        if (Input.touchCount > 0 && baseComponent.isHide == false)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && currentFingerId != touch.fingerId)
            {
                currentFingerId = touch.fingerId;
                TriggerTouch();
                return;
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

            if (coll.Raycast(ray, out hit, 100) && GamePlay.Instance.move > 0)
            {
                onBaseClickedForTutorial?.Invoke();
                baseComponent.OnBaseClicked();
            }
    }
}
