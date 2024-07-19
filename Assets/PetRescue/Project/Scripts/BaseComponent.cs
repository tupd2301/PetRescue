using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseComponent : MonoBehaviour
{
    [SerializeField] private GameObject _splashVFX;
    [SerializeField] private GameObject _swapUI;
    public GameObject main;
    public GameObject spawnPoint;
    public BaseData baseData;

    public bool isHide = false;

    public BaseType type;

    [SerializeField] private List<GameObject> baseModels = new List<GameObject>();
    [SerializeField] private List<GameObject> grassModels = new List<GameObject>();

    public void SetType(BaseType type)
    {
        this.type = type;
    }

    public void RandomGrassModel()
    {
        baseModels.ForEach(x => x.SetActive(false));
        grassModels.ForEach(x => x.SetActive(false));
        System.Random random = new System.Random();
        int index = random.Next(0, grassModels.Count);
        grassModels[index].SetActive(true);
    }
    public void SetModel(int id)
    {
        baseModels.ForEach(x => x.SetActive(false));
        grassModels.ForEach(x => x.SetActive(false));
        baseModels[id].SetActive(true);
        if (type == BaseType.SwapUpDown || type == BaseType.SwapLeftUp || type == BaseType.SwapLeftDown)
        {
            _swapUI.SetActive(true);
        }
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
        switch (type)
        {
            case BaseType baseType when baseType == BaseType.Normal || baseType == BaseType.Stop:
                GetComponentInChildren<Animator>().Play("Bounce");
                if (GamePlay.Instance?.petManager.CheckPetExist(baseData.coordinates) == false) return;
                PetComponent pet = GamePlay.Instance?.petManager?.GetPetByCoordinates(baseData.coordinates).petComponent;
                if (pet && !pet.isHide)
                {
                    pet.Run(GamePlay.Instance.baseManager.GetBaseDestination(pet.petData), pet.petData.baseCoordinates);
                    GamePlay.Instance.Move();
                }
                break;
            case BaseType baseType when baseType == BaseType.SwapUpDown || baseType == BaseType.SwapLeftUp || baseType == BaseType.SwapLeftDown:
                bool isSwap = GamePlay.Instance.baseManager.Swap(baseData);
                _swapUI.transform.DOLocalRotate(_swapUI.transform.localEulerAngles - new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360);
                GamePlay.Instance.Move();
                break;
            case BaseType baseType when baseType == BaseType.Hide:
                break;
            default:
                GetComponentInChildren<Animator>().Play("Bounce");
                break;
        }


    }
}
[System.Serializable]
public enum BaseType
{
    Hide,
    Normal,
    Stop,
    SwapUpDown,
    SwapLeftUp,
    SwapLeftDown
}