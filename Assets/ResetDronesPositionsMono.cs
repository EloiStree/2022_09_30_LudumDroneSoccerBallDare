using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDronesPositionsMono : MonoBehaviour
{

    public List<DroneToSpawn> m_droneToSpawn = new List<DroneToSpawn>();

    [System.Serializable]
    public class DroneToSpawn {

        public DroneSoccerSpawn5x5Tag m_spawn;
        public DroneSoccerRootTag m_droneRoot;
    }

    [ContextMenu("ResetPositions")]
    public void ResetPositions() {
        for (int i = 0; i < m_droneToSpawn.Count; i++)
        {
            if (m_droneToSpawn[i].m_droneRoot
                && m_droneToSpawn[i].m_droneRoot.GetRoot()
                && m_droneToSpawn[i].m_spawn
                && m_droneToSpawn[i].m_spawn.m_spawnRoot) { 
                m_droneToSpawn[i].m_droneRoot.GetRoot().position= m_droneToSpawn[i].m_spawn.m_spawnRoot.position;
            }

        }

    }

}
