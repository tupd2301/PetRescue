using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class EnvirontmentManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> envs = new List<GameObject>();

    public List<GameObject> props;
    

    public void Init()
    {
        envs.ForEach(x => x.SetActive(false));
        System.Random random = new System.Random();
        int index = random.Next(0, envs.Count);
        int[] ints = new int[4]{-2,-1,1,2};
        int randomID = random.Next(0, ints.Count());
        GameObject obj = Instantiate(envs[index], transform);
        Vector3 startBasePos = GamePlay.Instance.GetBaseEnvironment(ints[randomID]).obj.transform.position;
        Vector3 endBasePos = GamePlay.Instance.GetBaseEnvironment(-ints[randomID]).obj.transform.position;
        obj.transform.position = startBasePos;
        obj.SetActive(true);
        obj.transform.DORotate(new Vector3(0, 90, 0), 20f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        obj.transform.DOLocalMove(endBasePos, 20f).SetEase(Ease.Linear).OnComplete(() => {Destroy(obj);Init();});
    }
}
