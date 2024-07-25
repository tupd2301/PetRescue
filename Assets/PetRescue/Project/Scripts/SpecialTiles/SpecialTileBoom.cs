using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTileBoom : MonoBehaviour
{
    public void Explosion()
    {
        List<BaseData> list = GamePlay.Instance.baseManager.GetBaseNearByCoordinates(GetComponent<BaseComponent>().baseData.coordinates);
        list.Add(GetComponent<BaseComponent>().baseData);
        System.Random random = new System.Random();
        foreach (var item in list)
        {
            if(GamePlay.Instance.petManager.CheckPetExist(item.coordinates))
            {
                PetComponent petComponent = GamePlay.Instance.petManager.GetPetByCoordinates(item.coordinates).petComponent;
                petComponent.Next(item, petComponent.transform.position + new Vector3(random.Next(-1, 1), 0, random.Next(-1, 1)), true);
                petComponent.isHide = true;
            }
        }
        StartCoroutine(GamePlay.Instance.baseManager.SinkBases(list, 0.1f));

    }
}
