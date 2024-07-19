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

    public bool isHide;

    void Start()
    {
        isHide = false;
    }

    public void SetData(PetData petData)
    {
        this.petData = petData;
    }

    public void Next(BaseData baseData, Vector3 oriPos)
    {
        if (GamePlay.Instance.petManager.CheckPetExist(baseData.coordinates))
        {
            
            transform.DOLocalMoveY(3f, 0.5f).OnComplete(() =>
            {
                baseData.obj.GetComponent<BaseComponent>().CallSplashVFX();
                transform.DOLocalMoveY(-2f, 0.5f).OnComplete(() =>
                {
                    petData.petModelData.model.GetComponent<Animator>().Play("Run", -1);
                    transform.DOLocalMoveY(-0.3f, 0.3f).OnComplete(() =>
                    {
                        isHide = true;
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
        petData.baseCoordinates = data.First().Value.coordinates;
        BaseComponent baseComponent = data.First().Value.obj.GetComponent<BaseComponent>();
        Vector3 destination = new Vector3(baseComponent.spawnPoint.transform.position.x, 1, baseComponent.spawnPoint.transform.position.z);
        petData.baseCoordinates = originCoordinates;
        transform.DOMove(destination, 0.4f * (data.First().Key + 1))
                .OnUpdate(() => { if (Vector3.Distance(transform.position, destination) < 0.5f) petData.petModelData.model.GetComponent<Animator>().Play("Idle_A", -1); })
                .OnComplete(() => transform.DOMove(origin, 0.1f).OnComplete(() => petData.petModelData.model.GetComponent<Animator>().Play("Idle_A")));
    }

    public void Run(Dictionary<int, BaseData> data, Vector2 originCoordinates)
    {
        if (GamePlay.Instance.petManager.CheckPetExist(data.First().Value.coordinates) && !data.First().Value.obj.GetComponent<BaseComponent>().isHide)
        {
            Bounce(data, originCoordinates);
            return;
        }
        petData.petModelData.model.GetComponent<Animator>().Play("Roll");
        petData.baseCoordinates = data.First().Value.coordinates;
        BaseComponent baseComponent = data.First().Value.obj.GetComponent<BaseComponent>();
        Vector3 destination = new Vector3(baseComponent.spawnPoint.transform.position.x, 1, baseComponent.spawnPoint.transform.position.z);
        bool isJump = false;
        transform.DOMove(destination, 0.4f * (data.First().Key))
                .OnUpdate(() =>
                {
                    if (Vector3.Distance(transform.position, destination) < 4f && !isJump)
                    {
                        Next(data.First().Value, originCoordinates);
                        isJump = true;
                    }
                });
    }
}
