using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCControllerValueRelativeModeInputMono : MonoBehaviour
{
    public RCControllerValueMono m_input;
    public Transform m_transformToAffect;

    public float m_maxAngleTiltPerSeconds = 90;
    public float m_maxAngleYawPerSeconds = 90;
    public float m_maxAngleRollPerSeconds = 90;
    public float m_maxDistanceThrottlePerSeconds = 90;
    public void Update()
    {
        float dt = Time.deltaTime;
       
            m_transformToAffect.rotation *= Quaternion.Euler(0,
               m_maxAngleYawPerSeconds * m_input.m_controllerValue.m_yawPercentLeftToRight * dt,0);
        
       
            m_transformToAffect.rotation *= Quaternion.Euler(0,0,
              - m_maxAngleRollPerSeconds * m_input.m_controllerValue.m_rollPercentLeftToRight * dt) ;
        

            m_transformToAffect.rotation *= Quaternion.Euler(
               m_maxAngleTiltPerSeconds * m_input.m_controllerValue.m_tiltPercentDownToTop * dt, 0, 0);
        

            m_transformToAffect.position +=(m_transformToAffect.up*
               m_maxDistanceThrottlePerSeconds * m_input.m_controllerValue.m_throttlePercentDownToUp * dt);
        
    }
}
