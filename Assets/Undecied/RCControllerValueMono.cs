using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCControllerValueMono : MonoBehaviour
{
    public RCControllerValue m_controllerValue;
    public void SetYawPercent(float percent) { m_controllerValue.SetYawPercent( percent); }
    public void SetTiltPercent(float percent) { m_controllerValue.SetTiltPercent(percent); }
    public void SetRollPercent(float percent) { m_controllerValue.SetRollPercent(percent); }
    public void SetThrottlePercent(float percent) { m_controllerValue.SetThrottlePercent(percent); }

    public float GetTiltPercent()
    {
        return m_controllerValue.GetTiltPercent();
    }
    public float GetThrottlePercent()
    {
        return m_controllerValue.GetThrottlePercent();
    }

    public float GetYawPercent()
    {
        return m_controllerValue.GetYawPercent();
    }
    public float GetRollPercent()
    {
        return m_controllerValue.GetRollPercent();
    }
}

[System.Serializable]
public class RCControllerValue {
    [Range(-1, 1)]
    public float m_tiltPercentDownToTop;
    [Range(-1, 1)]
    public float m_yawPercentLeftToRight;
    [Range(-1, 1)]
    public float m_rollPercentLeftToRight;
    [Range(-1, 1)]
    public float m_throttlePercentDownToUp;

    public void SetYawPercent(float percent) { m_yawPercentLeftToRight = percent; }
    public void SetTiltPercent(float percent) { m_tiltPercentDownToTop = percent; }
    public void SetRollPercent(float percent) { m_rollPercentLeftToRight = percent; }
    public void SetThrottlePercent(float percent) { m_throttlePercentDownToUp = percent; }

    public float GetTiltPercent()
    {
        return m_tiltPercentDownToTop;
    }
    public float GetThrottlePercent()
    {
        return m_throttlePercentDownToUp;
    }
    public float GetYawPercent()
    {
        return m_yawPercentLeftToRight;
    }
    public float GetRollPercent()
    {
        return m_rollPercentLeftToRight;
    }
   
}