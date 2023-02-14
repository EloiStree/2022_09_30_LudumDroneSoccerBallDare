using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_CompressArenaPositionMono : MonoBehaviour
{
    public Transform m_arenaCenter;
    public Transform m_target;
    public Vector3 m_localPosition;
    public Vector3 m_localEuler;
    public CompressPositionSmallArea m_toSent;
    private void OnValidate()
    {
        Compress();

    }

    private void Compress()
    {
        Eloi.E_RelocationUtility.GetWorldToLocal_DirectionalPoint(m_target.position, m_target.rotation,
            in m_arenaCenter, out m_localPosition, out Quaternion q);
        m_localEuler = q.eulerAngles;

        ulong x = (ulong)(Mathf.Abs(m_localPosition.x) * 1000f) + (ulong)(m_localPosition.x>0?100000:0);
        ulong y = (ulong)(Mathf.Abs(m_localPosition.y) * 1000f) + (ulong)(m_localPosition.y > 0 ? 100000 : 0); ;
        ulong z = (ulong)(Mathf.Abs(m_localPosition.z) * 1000f) + (ulong)(m_localPosition.z > 0 ? 100000 : 0); ;
        m_toSent.m_position = (ulong)(x * 100000000000 + y * 100000 + z);

        x = (ulong)(Mathf.Abs(m_localEuler.x) * 100f);
        y = (ulong)(Mathf.Abs(m_localEuler.y) * 100f);
        z = (ulong)(Mathf.Abs(m_localEuler.z) * 100f);
        m_toSent.m_rotation = (ulong)(x * 10000000000 + y * 100000 + z);
    }

    private void Update()
    {

        Compress();
    }
}

public class CompressPositionSmallAreaUtility {


    public static void Compress(Transform root, Transform target, out CompressPositionSmallArea compressed)
    {

        compressed = new CompressPositionSmallArea();
        CompressInRef(root, target, ref compressed);
    }
    public static void CompressInRef(Transform root, Transform target, ref CompressPositionSmallArea compressed)
    {
        if(compressed==null)
            compressed = new CompressPositionSmallArea();
        Vector3 localPosition;
        Vector3 localEuler;
        Eloi.E_RelocationUtility.GetWorldToLocal_DirectionalPoint(target.position, target.rotation,
            in root, out localPosition, out Quaternion q);
        localEuler = q.eulerAngles;

        ulong x = (ulong)(Mathf.Abs(localPosition.x) * 1000f) + (ulong)(localPosition.x > 0 ? 100000 : 0);
        ulong y = (ulong)(Mathf.Abs(localPosition.y) * 1000f) + (ulong)(localPosition.y > 0 ? 100000 : 0);
        ulong z = (ulong)(Mathf.Abs(localPosition.z) * 1000f) + (ulong)(localPosition.z > 0 ? 100000 : 0);
        compressed.m_position = (ulong)(x * 100000000000 + y * 100000 + z);

        x = (ulong)(Mathf.Abs(localEuler.x) * 100f);
        y = (ulong)(Mathf.Abs(localEuler.y) * 100f);
        z = (ulong)(Mathf.Abs(localEuler.z) * 100f);
        compressed.m_rotation = (ulong)(x * 10000000000 + y * 100000 + z);
    }


}

[System.Serializable]
public class CompressPositionSmallArea {

    public ulong m_position;
    public ulong m_rotation;
}
