using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCControllerValueArcadeModeInputMono : MonoBehaviour
{
    public RCControllerValueMono m_input;
    public Transform m_transformToAffect;
    public Transform m_modeleRollPitch;

    public float m_maxLeftRight=1;
    public float m_maxBackFoward=1;
    public float m_maxAngleTiltPerSeconds = 35;
    public float m_maxAngleYawPerSeconds = 90;
    public float m_maxAngleRollPerSeconds = 35;
    public float m_maxDistanceThrottlePerSeconds = 2;

    public void Update()
    {
        float dt = Time.deltaTime;
       
            m_transformToAffect.rotation = Quaternion.Euler(0,
               m_maxAngleYawPerSeconds * m_input.m_controllerValue.m_yawPercentLeftToRight * dt, 0) * m_transformToAffect.rotation;
        
       
            m_modeleRollPitch.localRotation = Quaternion.Euler( m_maxAngleTiltPerSeconds * 
                m_input.m_controllerValue.m_tiltPercentDownToTop,0 ,
              - m_maxAngleRollPerSeconds * m_input.m_controllerValue.m_rollPercentLeftToRight );
           
        


        Vector3 move = new Vector3(

             m_maxLeftRight * m_input.m_controllerValue.m_rollPercentLeftToRight * dt
            , m_maxDistanceThrottlePerSeconds * m_input.m_controllerValue.m_throttlePercentDownToUp * dt
            , m_maxBackFoward * m_input.m_controllerValue.m_tiltPercentDownToTop * dt
               );
      Vector3 dir=  m_transformToAffect.forward;
        dir.y= 0;
        Quaternion flatRotation =  Quaternion.LookRotation( dir, Vector3.up);

        Vector3 foward = (flatRotation * Vector3.forward) * 
              m_maxBackFoward* m_input.m_controllerValue. m_tiltPercentDownToTop* dt;
        Vector3 right = (flatRotation * Vector3.right) *
            m_maxLeftRight * m_input.m_controllerValue.m_rollPercentLeftToRight * dt;
        Vector3 up = (flatRotation * Vector3.up) *
            m_maxDistanceThrottlePerSeconds * m_input.m_controllerValue.m_throttlePercentDownToUp * dt;

        m_transformToAffect.position += foward;
        m_transformToAffect.position += right;
        m_transformToAffect.position += up;
    }

}
