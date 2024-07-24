using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTileLock : MonoBehaviour
{

    private int _count;
    public int count
    {
        get
        {
            return _count;
        }
        set
        {
            _count = value;
            gameObject.GetComponent<BaseComponent>().SetLockText(_count);
        }
    }
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
