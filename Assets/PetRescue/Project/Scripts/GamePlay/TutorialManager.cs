using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<GameObject> hands = new List<GameObject>();
    public Dictionary<string, int> listTrigger = new Dictionary<string, int>();

    void Start()
    {
        InitTriggers();
    }

    public void InitTriggers()
    {
        listTrigger = new Dictionary<string, int>();
        listTrigger.Add("lvl9", 0);
        listTrigger.Add("lvl27", 0);
        listTrigger.Add("lvl32", 0);
        listTrigger.Add("lvl44", 0);
    }

    public void ClearAll()
    {
        foreach (var item in hands)
        {
            item.transform.SetParent(transform);
            item.SetActive(false);
        }
    }
    public void ShowTutorial()
    {
        ClearAll();
        switch (GamePlay.Instance.currentLevelData.level)
        {
            case 0:
                PetData petData = GamePlay.Instance.petManager.pets.FirstOrDefault(x => x.petComponent.petData.isHide == false);
                if (petData != null && petData.petComponent != null)
                {
                    SpawnHand(petData.petComponent.transform);
                }
                break;
            case 9:
                if (listTrigger["lvl9"] == 0)
                {
                    BaseData baseData = GamePlay.Instance.baseManager.bases
                    .FirstOrDefault(x => x.obj.GetComponent<BaseComponent>().baseData.type == BaseType.SwapUpDown
                    || x.obj.GetComponent<BaseComponent>().baseData.type == BaseType.SwapLeftUp
                    || x.obj.GetComponent<BaseComponent>().baseData.type == BaseType.SwapLeftDown);
                    if (baseData != null)
                    {
                        baseData.obj.GetComponentInChildren<BaseTrigger>().onBaseClickedForTutorial += () =>
                        {
                            GamePlay.Instance.tutorialManager.listTrigger["lvl9"] = 1;
                            GamePlay.Instance.tutorialManager.ClearAll();
                            baseData.obj.GetComponentInChildren<BaseTrigger>().onBaseClickedForTutorial = null;
                        };
                        SpawnHand(baseData.obj.transform);
                    }
                }
                break;
            case 27:
                if (listTrigger["lvl27"] == 0)
                {
                    BaseData baseData = GamePlay.Instance.baseManager.bases.FirstOrDefault(x => x.coordinates == new Vector2(-1, 0));
                    if (baseData != null && GamePlay.Instance.petManager.CheckPetExist(baseData.coordinates))
                    {
                        baseData.obj.GetComponentInChildren<BaseTrigger>().onBaseClickedForTutorial += () =>
                        {
                            GamePlay.Instance.tutorialManager.listTrigger["lvl27"] = 1;
                            GamePlay.Instance.tutorialManager.ClearAll();
                            baseData.obj.GetComponentInChildren<BaseTrigger>().onBaseClickedForTutorial = null;
                        };
                        SpawnHand(baseData.obj.transform);
                    }
                }
                break;
            case 32:
                if (listTrigger["lvl32"] == 0)
                {
                    BaseData baseData = GamePlay.Instance.baseManager.bases.FirstOrDefault(x => x.coordinates == new Vector2(1, 0));
                    if (baseData != null && GamePlay.Instance.petManager.CheckPetExist(baseData.coordinates))
                    {
                        baseData.obj.GetComponentInChildren<BaseTrigger>().onBaseClickedForTutorial += () =>
                        {
                            GamePlay.Instance.tutorialManager.listTrigger["lvl32"] = 1;
                            GamePlay.Instance.tutorialManager.ClearAll();
                            baseData.obj.GetComponentInChildren<BaseTrigger>().onBaseClickedForTutorial = null;
                        };
                        SpawnHand(baseData.obj.transform);
                    }
                }
                break;
            case 44:
                if (listTrigger["lvl44"] == 0)
                {
                    BaseData baseData = GamePlay.Instance.baseManager.bases.FirstOrDefault(x => x.coordinates == new Vector2(-1, 0));
                    if (baseData != null)
                    {
                        baseData.obj.GetComponentInChildren<BaseTrigger>().onBaseClickedForTutorial += () =>
                        {
                            GamePlay.Instance.tutorialManager.listTrigger["lvl44"] = 1;
                            GamePlay.Instance.tutorialManager.ClearAll();
                            baseData.obj.GetComponentInChildren<BaseTrigger>().onBaseClickedForTutorial = null;
                        };
                        SpawnHand(baseData.obj.transform);
                    }
                }
                break;
            default:
                break;
        }
    }
    public void SpawnHand(Transform target)
    {
        if (target != null)
        {
            if (hands.FindAll(x => x.activeInHierarchy == false).Count() == 0)
            {
                GameObject hand = Instantiate(Resources.Load<GameObject>("Prefabs/UIPrefabs/TutorialIndicator"), transform);
                hand.transform.position = new Vector3(target.position.x, 0, target.position.z);
                hand.transform.localEulerAngles = new Vector3(0, 0, 0);
                hand.transform.localScale = Vector3.one;
                hands.Add(hand);
            }
            else
            {
                GameObject hand = hands.FirstOrDefault(x => x.activeInHierarchy == false);
                if (hand != null)
                {
                    hand.SetActive(true);
                    hand.transform.position = new Vector3(target.position.x, 0, target.position.z);
                    hand.transform.localEulerAngles = new Vector3(0, 0, 0);
                    hand.transform.localScale = Vector3.one;
                }
            }
        }
    }
}
