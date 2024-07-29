using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTileLock : MonoBehaviour
{
    public int count
    {
        get
        {
            if(GetComponent<BaseComponent>().baseData.listPara.ContainsKey("countLock") == false)
            {
                GetComponent<BaseComponent>().baseData.listPara.Add("countLock", 0);
            }
            return (int)GetComponent<BaseComponent>().baseData.listPara["countLock"];
        }
        set
        {
            if(GetComponent<BaseComponent>().baseData.listPara.ContainsKey("countLock") == false)
            {
                GetComponent<BaseComponent>().baseData.listPara.Add("countLock", value);
            }
            GetComponent<BaseComponent>().baseData.listPara["countLock"] = value;
            gameObject.GetComponent<BaseComponent>().SetLockText(value);
        }
    }
    public bool isUnlocked = false;
    public void Unlock()
    {
        count--;
        if (count == 0)
        {
            isUnlocked = true;
            gameObject.GetComponent<BaseComponent>().SetModel("normal");
        }
    }
}
