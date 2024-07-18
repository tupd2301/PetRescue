using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEditor.iOS;
using System.Linq;

public class PetComponent : MonoBehaviour
{
    public PetData petData;
    public Action<BaseData> OnCompleteRun;

    public bool isHide = false;

    void Start()
    {
        isHide = false;
        OnCompleteRun = (BaseData baseData) => { Next(baseData); };
    }

    public void SetData(PetData petData)
    {
        this.petData = petData;
    }

    public void Next(BaseData baseData)
    {
        if (GamePlay.Instance.petManager.CheckPetExist(baseData.coordinates))
        {
            isHide = true;
            transform.DOLocalMoveY(-0.3f, 0.3f);
        }
    }

    public void Bounce(Dictionary<int, BaseData> data)
    {
        Vector3 origin = transform.position;
        petData.petModelData.model.GetComponent<Animator>().Play("Roll");
        petData.baseCoordinates = data.First().Value.coordinates;
        BaseComponent baseComponent = data.First().Value.obj.GetComponent<BaseComponent>();
        Vector3 destination = new Vector3(baseComponent.spawnPoint.transform.position.x, 1, baseComponent.spawnPoint.transform.position.z);
        transform.DOMove(destination, 0.3f * (data.First().Key + 1))
                .OnUpdate(() => { if (Vector3.Distance(transform.position, destination) < 0.5f) petData.petModelData.model.GetComponent<Animator>().Play("Idle_A", -1); })
                .OnComplete(() => transform.DOMove(origin, 0.1f).OnComplete(()=> petData.petModelData.model.GetComponent<Animator>().Play("Idle_A")));
    }

    public void Run(Dictionary<int, BaseData> data)
    {
        if(GamePlay.Instance.petManager.CheckPetExist(data.First().Value.coordinates))
        {
            Bounce(data);
            return;
        }
        petData.petModelData.model.GetComponent<Animator>().Play("Roll");
        petData.baseCoordinates = data.First().Value.coordinates;
        BaseComponent baseComponent = data.First().Value.obj.GetComponent<BaseComponent>();
        Vector3 destination = new Vector3(baseComponent.spawnPoint.transform.position.x, 1, baseComponent.spawnPoint.transform.position.z);
        transform.DOMove(destination, 0.3f * (data.First().Key + 1))
                .OnUpdate(() => { if (Vector3.Distance(transform.position, destination) < 0.5f) petData.petModelData.model.GetComponent<Animator>().Play("Idle_A", -1); })
                .OnComplete(() => OnCompleteRun?.Invoke(data.First().Value));
    }
}
