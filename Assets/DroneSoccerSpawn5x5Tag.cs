using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSoccerSpawn5x5Tag : MonoBehaviour
{
    public DroneSoccer5x5ID m_droneId;


    public Transform m_spawnRoot;

    public void Reset()
    {
        SetRootWithDefault();
    }

    [ContextMenu("SetRootWithDefault")]
    public void SetRootWithDefault() {

        m_spawnRoot = this.transform;
    }
}

