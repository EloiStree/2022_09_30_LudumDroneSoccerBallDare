using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGoalPositionMono : MonoBehaviour
{
    public Transform m_backMiddleAnchor;
    public Transform m_whatToMove;
    public float m_distanceFromBackMeter =1f;
    public float m_distanceFromGroundMeter=1.5f;

    [ContextMenu("Refresh")]
    public void RefreshPosition()
    {
        if(m_backMiddleAnchor && m_whatToMove)
            m_whatToMove.position = m_backMiddleAnchor.TransformPoint(new Vector3(0, m_distanceFromGroundMeter, m_distanceFromBackMeter));
    }

    private void OnValidate()
    {
        RefreshPosition();
    }
}
