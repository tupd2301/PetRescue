using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHandler : MonoBehaviour
{
    public bool m_bAudioEnabled = true;

    void Start()
    {
        AudioHandler.SetEnabledState(m_bAudioEnabled);
    }

    void Update()
    {

    }
}
