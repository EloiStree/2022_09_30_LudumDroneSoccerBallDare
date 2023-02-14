using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LudumEvery10SecondsCountMono : MonoBehaviour
{
    public float m_timeLeft=10f;
    public UnityEvent m_onTimeReach;
    public Eloi.PrimitiveUnityEvent_Float m_timeLeftChanged;
    public Eloi.PrimitiveUnityEvent_Float m_timeCountChanged;
    public Eloi.PrimitiveUnityEvent_Float m_timePercentLoadChanged;
    private void Update()
    {
        if (m_timeLeft > 0f) {
            m_timeLeft -= Time.deltaTime;

            if (m_timeLeft <= 0f) {
                m_onTimeReach.Invoke();
                m_timeLeft = 10;
            }
            m_timeLeftChanged.Invoke(m_timeLeft);
            m_timeCountChanged.Invoke(10f - m_timeLeft);
            m_timePercentLoadChanged.Invoke(m_timeLeft/10f);
        }
    }


}
