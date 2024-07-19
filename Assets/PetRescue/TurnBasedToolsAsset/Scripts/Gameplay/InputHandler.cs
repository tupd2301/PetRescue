using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class KeyEvent : UnityEvent<int>
{ }

[System.Serializable]
public struct InputHandler
{
    [HideInInspector]
    public KeyEvent OnNumPressed;

    public void Update()
    {
        int numPressed = -1;
        if (IsNumKeyPressed(ref numPressed))
        {
            if(numPressed != -1)
            {
                OnNumPressed.Invoke(numPressed);
            }
        }
    }

    bool IsNumKeyPressed(ref int Outnum)
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Outnum = 1;
            return true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Outnum = 2;
            return true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Outnum = 3;
            return true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            Outnum = 4;
            return true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            Outnum = 5;
            return true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            Outnum = 6;
            return true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            Outnum = 7;
            return true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            Outnum = 8;
            return true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            Outnum = 9;
            return true;
        }

        return false;
    }
}
