using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour
{
    [SerializeField] private GameObject _splashVFX;
    public GameObject main;
    public GameObject spawnPoint;
    public BaseData baseData;

    public bool isHide = false;

    public bool isStop = false;

    [SerializeField] private List<GameObject> baseModels = new List<GameObject>();

    public void RandomModel()
    {
        baseModels.ForEach(x => x.SetActive(false));
        int index = Random.Range(0, baseModels.Count);
        baseModels[index].SetActive(true);
    }
    public void CallSplashVFX()
    {
        _splashVFX.SetActive(true);
    }
    public void SetModelSand()
    {
        baseModels.ForEach(x => x.SetActive(false));
        baseModels[0].SetActive(true);
    }

    public void OnBaseClicked()
    {
        GetComponentInChildren<Animator>().Play("Bounce");
        if(GamePlay.Instance?.petManager.CheckPetExist(baseData.coordinates) == false) return;
        PetComponent pet = GamePlay.Instance?.petManager?.GetPetByCoordinates(baseData.coordinates);
        if(pet && !pet.isHide) pet.Run(GamePlay.Instance.baseManager.GetBaseDestination(pet.petData), pet.petData.baseCoordinates);
    }
}
