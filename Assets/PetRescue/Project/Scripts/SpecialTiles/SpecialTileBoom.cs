using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTileBoom : MonoBehaviour
{
    public void Explosion()
    {
        List<BaseData> list = GamePlay.Instance.baseManager.GetBaseNearByCoordinates(GetComponent<BaseComponent>().baseData.coordinates);
        list.Add(GetComponent<BaseComponent>().baseData);
        StartCoroutine(GamePlay.Instance.baseManager.SinkBases(list, 0.1f));
    }
}
