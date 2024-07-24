using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTileBoom : MonoBehaviour
{
    public int count;
    public bool isUnlocked = false;
    public void Explosion()
    {
        count--;
        Debug.Log(count);
        if (count == 0)
        {
            isUnlocked = true;
            gameObject.GetComponent<BaseComponent>().SetModel("normal");
        }
    }
}
