using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSoccerSpawn3x3Tag : MonoBehaviour
{
    public DroneSoccer3x3ID m_droneId;
    public Transform m_spawnRoot;

    public void Reset()
    {
        SetRootWithDefault();
    }

    [ContextMenu("SetRootWithDefault")]
    public void SetRootWithDefault()
    {

        m_spawnRoot = this.transform;
    }
}
