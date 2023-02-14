using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDroneColorMono : MonoBehaviour
{
    public Eloi.ClassicUnityEvent_Color m_colorToAffectToDrone;

    public void SetColorToAffectAtDrone(Color color)
    {
        m_colorToAffectToDrone.Invoke(color);
    }
    [ContextMenu("Random Color")]
    public void SetRandomColor()
    {
        Eloi.E_UnityRandomUtility.GetRandomColor(out Color color);
        SetColorToAffectAtDrone(color);
    }
}
