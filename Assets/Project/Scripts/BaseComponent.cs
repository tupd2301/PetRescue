using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour
{
    public GameObject spawnPoint;
    public BaseData baseData;

    public bool isHide = false;

    [SerializeField] private List<GameObject> baseModels = new List<GameObject>();

    void Start()
    {
        isHide = false;
    }
    public void RandomModel()
    {
        baseModels.ForEach(x => x.SetActive(false));
        int index = Random.Range(1, baseModels.Count);
        baseModels[index].SetActive(true);
    }
    public void SetModelSand()
    {
        baseModels.ForEach(x => x.SetActive(false));
        baseModels[0].SetActive(true);
    }

    public void OnBaseClicked()
    {
        GetComponentInChildren<Animator>().Play("Bounce");
        PetComponent pet = GamePlay.Instance?.petManager?.GetPetByCoordinates(baseData.coordinates);
        if(pet && !pet.isHide) pet.Run(GamePlay.Instance.baseManager.GetBaseDestination(pet.petData));
    }
}
