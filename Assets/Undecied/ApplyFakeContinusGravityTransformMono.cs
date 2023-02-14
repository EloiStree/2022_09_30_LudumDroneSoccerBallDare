using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFakeContinusGravityTransformMono : MonoBehaviour
{

    public Transform m_toAffect;
    public bool m_isGravityEnable = true;
    public float m_gravityToApply=9.81f;

    public void Update()
    {
        if(m_isGravityEnable)
            m_toAffect.position += Vector3.down*Time.deltaTime*m_gravityToApply;
    }
}
