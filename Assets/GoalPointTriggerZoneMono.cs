using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GoalPointTriggerZoneMono : MonoBehaviour
{
    public Transform m_goalDirection;
    public Dictionary<DroneSoccerRootTag, DroneCollisionWithEntry> m_dronesInCollision = new Dictionary<DroneSoccerRootTag, DroneCollisionWithEntry>();
    public List<DroneCollisionWithEntry> m_debugCollisionList = new List<DroneCollisionWithEntry>();

    public int          m_pointCountForDebug;
    public UnityEvent   m_onValideGoal;


    [System.Serializable]
    public class DroneCollisionWithEntry {
        public DroneSoccerRootTag m_drone;
        public Vector3 m_rootWhenEnterPosition;
        public Vector3 m_rootWhenExitPosition;

        public DroneCollisionWithEntry(){}
        public DroneCollisionWithEntry(DroneSoccerRootTag drone) { m_drone = drone; }
    }
    public void OnTriggerEnter(Collider other)
    {
        DroneSoccerRootTag script = other.GetComponentInChildren<DroneSoccerRootTag>();
        if (script != null)
        {
            if (!m_dronesInCollision.ContainsKey(script))
                m_dronesInCollision.Add(script, new DroneCollisionWithEntry(script));
            m_dronesInCollision[script].m_rootWhenEnterPosition = script.GetRoot().position;
            RefreshList();
        }
    }
    private void OnTriggerExit(Collider other)
    {

        DroneSoccerRootTag script = other.GetComponentInChildren<DroneSoccerRootTag>();
        if (script != null)
        {
            if (!m_dronesInCollision.ContainsKey(script))
                m_dronesInCollision.Add(script, new DroneCollisionWithEntry(script));
            m_dronesInCollision[script].m_rootWhenExitPosition = script.GetRoot().position;
            RefreshList();

            DroneCollisionWithEntry record = m_dronesInCollision[script];
            bool wasFrontEntry = m_goalDirection.InverseTransformPoint(record.m_rootWhenEnterPosition).z > 0;
            bool wasBackExit = m_goalDirection.InverseTransformPoint(record.m_rootWhenExitPosition).z <= 0;
            if (wasFrontEntry && wasBackExit) {
                m_pointCountForDebug++;
                m_onValideGoal.Invoke();
            }
        }
    }

   

    private void RefreshList()
    {
        m_debugCollisionList = m_dronesInCollision.Values.ToList() ;
    }

}
