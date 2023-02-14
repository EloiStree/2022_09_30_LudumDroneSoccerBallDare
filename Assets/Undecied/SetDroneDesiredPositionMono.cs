using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDroneDesiredPositionMono : MonoBehaviour
{
    public Transform m_whereToBeAnchor;
    public Vector3 m_whereToBeWanted;
    public Transform m_droneRoot;
    public Rigidbody m_droneRigidBody;

    [Range(0,1)]
    public float m_movePowerSpeed;
    public float m_accelerationForce =0.2f;
    public float m_accelerationForceMultiplicator =1f;
    public ForceMode m_forceMode;

    void Update()
    {
        if (m_whereToBeAnchor)
            m_whereToBeWanted = m_whereToBeAnchor.position;

        Vector3 directionNormalized = (m_whereToBeWanted - m_droneRoot.position).normalized;
        directionNormalized *= m_accelerationForce;
        m_droneRigidBody.AddForce(directionNormalized * m_accelerationForce * m_accelerationForceMultiplicator * m_movePowerSpeed, m_forceMode);


    }
}
