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
        if (Input.GetMouseButtonDown(0) && baseComponent.isHide == false)
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
}
