using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObj : MonoBehaviour
{
    public Vector3 RotateDir;
    
    void Update()
    {
        transform.Rotate(RotateDir);
    }
}
