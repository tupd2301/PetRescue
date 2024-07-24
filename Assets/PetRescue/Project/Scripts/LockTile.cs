using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTile : MonoBehaviour
{
    public int count;
    public bool isUnlocked = false;
    public void Unlock()
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
