using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverObj : MonoBehaviour
{
    public float m_Height;
    
    void Update()
    {
        float deltaHeight = Mathf.Sin(Time.time + Time.deltaTime) - Mathf.Sin(Time.time);

        transform.position += new Vector3(0, deltaHeight * m_Height, 0);
    }
}
