using System;
using System.Collections;
using System.Collections.Generic;
using TNet;
using UnityEngine;
using UnityEngine.Events;

public class TN_FirstDeviceIsServerMono : MonoBehaviour
{
    public Eloi.PrimitiveUnityEventExtra_Bool m_newUserIsServer= new Eloi.PrimitiveUnityEventExtra_Bool();
    public TN_DeviceTargetMono m_serverDevice;
    void Awake()
    {
        TN_DeviceTargetMono.m_onNewDeviceDetected += NotifyNewDevice;
    }
    private void OnEnable()
    {

        TN_DeviceTargetMono.m_onNewDeviceDetected -= NotifyNewDevice;
        TN_DeviceTargetMono.m_onNewDeviceDetected += NotifyNewDevice;
    }
    private void OnDisable()
    {

        TN_DeviceTargetMono.m_onNewDeviceDetected -= NotifyNewDevice;
    }

    private void NotifyNewDevice(TN_DeviceTargetMono obj)
    {
        if (obj.IsMine()) 
        Invoke("NotifyNewDeviceDelay", 0.1f);
    }
    private void NotifyNewDeviceDelay()
    {
        int count = TN_DeviceTargetMono.GetCountInScene();
        m_newUserIsServer.Invoke(count <= 1);
    }


}
