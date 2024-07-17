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

    void Start()
    {
        OnCompleteRun = (BaseData baseData) => { Next(baseData);};
    }

    public void SetData(PetData petData)
    {
        this.petData = petData;
    }

    public void Next(BaseData baseData)
    {
        if(GamePlay.Instance.petManager.CheckPetExist(baseData.coordinates))
        {
            Debug.Log("Pet Stop");
            GetComponent<Animator>().Play("Jump");
            // GamePlay.Instance.petManager.GetPetByCoordinates(baseData.coordinates).Run(GamePlay.Instance.baseManager.GetBaseDestination(petData));
        }
    }


    public void Run(Dictionary<int, BaseData> data)
    {
        petData.petModelData.model.GetComponent<Animator>().PlayInFixedTime("Roll");
        petData.baseCoordinates = data.First().Value.coordinates;
        BaseComponent baseComponent = data.First().Value.obj.GetComponent<BaseComponent>();
        Vector3 destination = new Vector3(baseComponent.spawnPoint.transform.position.x, 1, baseComponent.spawnPoint.transform.position.z);
        transform.DOLocalMove(destination, 0.3f*(data.First().Key)).OnUpdate(() => { if(Vector3.Distance(transform.position, destination) < 1f) petData.petModelData.model.GetComponent<Animator>().Play("Idle_A",-1);}).OnComplete(() => OnCompleteRun?.Invoke(data.First().Value));
    }
}
