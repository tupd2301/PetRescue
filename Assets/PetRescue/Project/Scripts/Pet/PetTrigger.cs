using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PetTrigger : MonoBehaviour
{
    public PetComponent petComponent;
    private Collider coll;
    void Start()
    {
        coll = petComponent.petData.petModelData.model.GetComponent<Collider>();
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                TriggerTouch();
            }
        }
        if (Input.GetMouseButtonDown(0))
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
            if(GetComponent<PetComponent>().isHide == false || petComponent.isBusy == true) return;
            Transform t = petComponent.petData.petModelData.model.transform;
            int rotateX = t.localEulerAngles.x == -150f ? 0 : -150;
            t.DOLocalMoveY(-2.5f, 0.3f).OnComplete(() => t.DOLocalMoveY(rotateX == -150 ? 1.3f : -0.5f, 0.3f));
            t.DOLocalRotate(new Vector3(rotateX, t.localEulerAngles.y, t.localEulerAngles.z), 0.3f);
        }
    }
}
