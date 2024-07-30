using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

public class PetComponent : MonoBehaviour
{
    [SerializeField] private GameObject _ripplesVFX;
    public PetData petData;

    

    void Start()
    {
        petData.isHide = false;
        petData.isBusy = false;
    }

    public void SetData(PetData petData)
    {
        this.petData = petData;
    }

    public void Next(BaseData baseData, Vector3 oriPos, bool isForce = false)
    {
        // if (GamePlay.Instance.petManager.CheckPetExist(baseData.coordinates) || isForce)
        {
            transform.DOLocalMoveY(3f, 0.5f).OnComplete(() =>
            {
                Debug.Log("Next");
                // baseData.obj.GetComponent<BaseComponent>().CallSplashVFX();
                SoundManager.Instance.PlaySound("splash");
                Handheld.Vibrate();
                transform.DOLocalMoveY(-2f, 0.5f).OnComplete(() =>
                {
                    petData.petModelData.model.GetComponent<Animator>().Play("Run", -1);
                    transform.DOLocalMoveY(-0.1f, 0.3f).OnComplete(() =>
                    {
                        GamePlay.Instance.CheckWin();
                        _ripplesVFX.SetActive(true);
                        Vector3 des = transform.position + (transform.position - oriPos);
                        float distance = Vector3.Distance(transform.position, des);
                        transform.DOLocalRotate(new Vector3(0, transform.eulerAngles.x + 50, 0), 1f).SetLoops(-1, LoopType.Incremental);
                        transform.DOMove(des, 10f * (distance / 3f))
                        .OnUpdate(() =>
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x, -0.3f, transform.localPosition.z);
                        })
                        .SetLoops(20, LoopType.Incremental)
                        .OnComplete(() => gameObject.SetActive(false));
                    });
                });
            });

        }
    }

    public void Bounce(Dictionary<int, BaseData> data, Vector2 originCoordinates)
    {
        Vector3 origin = transform.position;
        petData.petModelData.model.GetComponent<Animator>().Play("Roll");
        // petData.baseCoordinates = data.First().Value.coordinates;
        BaseComponent baseComponent = data.First().Value.obj.GetComponent<BaseComponent>();
        Vector3 destination = new Vector3(baseComponent.spawnPoint.transform.position.x, 1, baseComponent.spawnPoint.transform.position.z);
        // petData.baseCoordinates = originCoordinates;
        Vector3 direction = (destination - transform.position).normalized;
        transform.DOMove(destination - direction * 1, 0.4f * (data.First().Key - 1))
                .OnComplete(() =>
                {
                    Handheld.Vibrate();
                    if (baseComponent.GetComponent<SpecialTileBoom>() != null)
                    {
                        baseComponent.GetComponent<SpecialTileBoom>().Explosion();
                        petData.isHide = true;
                        GamePlay.Instance.OnPetJump?.Invoke();
                        Next(data.First().Value, origin, true);
                        return;
                    }
                    SoundManager.Instance.PlaySound("collide");
                    PetData petDataExist = GamePlay.Instance.petManager.GetPetByCoordinates(data.First().Value.coordinates);
                    if (petDataExist != null) petDataExist.petModelData.model.GetComponent<Animator>().Play("Bounce");
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                    transform.DOMove(origin, 1f)
                    .OnComplete(() =>
                    {
                        transform.localEulerAngles = new Vector3(0, 0, 0);
                        petData.petModelData.model.GetComponent<Animator>().Play("Idle_A");
                        petData.isBusy = false;
                    });
                });
    }

    public void Run(Dictionary<int, BaseData> data, Vector2 originCoordinates)
    {
        petData.isBusy = true;
        GameObject obj = data.First().Value.obj;
        if ((GamePlay.Instance.petManager.CheckPetExist(data.First().Value.coordinates))
                || (obj.GetComponent<BaseComponent>().baseData.type == BaseType.Lock && !obj.GetComponent<SpecialTileLock>().isUnlocked && obj.GetComponent<BaseComponent>().baseData.isHide == false)
                || (obj.GetComponent<BaseComponent>().baseData.type == BaseType.Boom && obj.GetComponent<BaseComponent>().baseData.isHide == false)
                || obj.GetComponent<BaseComponent>().baseData.type == BaseType.SwapUpDown
                || obj.GetComponent<BaseComponent>().baseData.type == BaseType.SwapLeftUp
                || obj.GetComponent<BaseComponent>().baseData.type == BaseType.SwapLeftDown)
        {
            Bounce(data, originCoordinates);
            return;
        }
        petData.petModelData.model.GetComponent<Animator>().Play("Roll");
        petData.baseCoordinates = data.First().Value.coordinates;
        BaseComponent baseComponent = data.First().Value.obj.GetComponent<BaseComponent>();
        bool isStop = data.First().Value.obj.GetComponent<BaseComponent>().baseData.type == BaseType.Stop;
        Vector3 destination = new Vector3(baseComponent.spawnPoint.transform.position.x, 1, baseComponent.spawnPoint.transform.position.z);
        bool isJump = isStop;
        System.Random random = new System.Random();
        if (random.Next(0, 100) < 100)
            SoundManager.Instance.PlaySound("preRoll");

        GamePlay.Instance.OnPetJump?.Invoke();
        if(!isStop)petData.isHide = true;

        transform.DOLocalMoveY(5, 0.3f).OnComplete(() =>
        {
            transform.DOLocalMoveY(1.2f, 0.3f).OnComplete(() =>
            {
                transform.DOMove(destination, 0.4f * (data.First().Key))
                    .OnUpdate(() =>
                    {
                        if (Vector3.Distance(transform.position, destination) < 4f && !isJump)
                        {
                            Next(data.First().Value, originCoordinates);
                            isJump = true;
                        }
                    })
                    .OnComplete(() =>
                    {
                        petData.isBusy = false;
                        if (isStop)
                        {
                            petData.petModelData.model.GetComponent<Animator>().Play("Idle_A");
                        }
                    });
            });
        }
        );
    }
}
