using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndSpeedMoveMono : MonoBehaviour
{
    public Transform m_whatToMove;
    public Vector3 m_wantedPosition;
    [Range(0f,1f)]
    public float m_percentSpeed;
    public float m_maxSpeed = 2;

    public float m_deathZone = 0.05f;


    public void Update()
    {
        Vector3 direction = m_wantedPosition - m_whatToMove.position;
        Vector3 newPosition = m_whatToMove.position + direction.normalized * m_maxSpeed* m_percentSpeed * Time.deltaTime;
        Vector3 directionFuture = m_wantedPosition - newPosition;
        if (directionFuture.magnitude > direction.magnitude)
            newPosition = m_whatToMove.position + direction;
        m_whatToMove.position = newPosition;
    }

}
