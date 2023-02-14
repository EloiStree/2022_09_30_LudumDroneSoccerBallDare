using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TNet;
using UnityEngine;


[RequireComponent(typeof(TNObject))]
public class TN_DeviceTargetMono : TNBehaviour
{
    public static System.Collections.Generic.List<TN_DeviceTargetMono> m_listOfDevice = new System.Collections.Generic.List<TN_DeviceTargetMono>();
    public static TN_DeviceTargetMono Mine;
    public static Action<TN_DeviceTargetMono> m_onNewDeviceDetected;

    public string m_deviceId;

    public bool IsMine()
    {
        return tno.isMine;
    }

    public string m_deviceName;

    public static int GetCountInScene()
    {
        return m_listOfDevice.Count;
    }

    public string m_deviceDescription;
    public string [] m_deviceAliasNames;
    public ulong m_uniqueId;

    private void OnEnable()
    {
        if (tno.isMine)
        {
            Mine = this;
        }
        m_listOfDevice.Add(this);
        if(m_onNewDeviceDetected!=null)
            m_onNewDeviceDetected.Invoke(this);
    }
    private void OnDisable()
    {

        m_listOfDevice.Remove(this);
        m_listOfDevice = m_listOfDevice.Where(k => k != null).ToList();
    }

    public void Start()
    {
        

        Eloi.E_GeneralUtility.GetTimeULongId(DateTime.Now, out m_uniqueId);
        System.Collections.Generic.List<string> info = new System.Collections.Generic.List<string>();
        info.Add(SystemInfo.deviceModel);
        info.Add(SystemInfo.deviceType.ToString());
        info.Add(SystemInfo.processorCount.ToString());
        info.Add(SystemInfo.processorFrequency.ToString());
        info.Add(SystemInfo.processorType);
        info.Add(SystemInfo.graphicsDeviceID.ToString());
        info.Add(SystemInfo.graphicsDeviceName);
        info.Add(SystemInfo.graphicsDeviceType.ToString());
        info.Add(SystemInfo.graphicsDeviceVendor);
        info.Add(SystemInfo.graphicsDeviceVendorID.ToString());
        info.Add(SystemInfo.graphicsDeviceVersion);
        info.Add(SystemInfo.graphicsMemorySize.ToString());
        info.Add(SystemInfo.graphicsMultiThreaded.ToString());
        info.Add(SystemInfo.operatingSystem);
        info.Add(SystemInfo.operatingSystemFamily.ToString());
        m_deviceId = SystemInfo.deviceUniqueIdentifier;
        m_deviceName = SystemInfo.deviceName;
        m_deviceDescription = string.Join("---" , info);
    }

    public void SetDeviceId(string value) { tno.Send(0, Target.AllSaved, value); }
    public void SetDeviceName(string value) { tno.Send(1, Target.AllSaved, value); }
    public void SetDeviceModelLog(string value) { tno.Send(3, Target.AllSaved, value); }
    public void SetDeviceAlias(string[] value) { tno.Send(2, Target.AllSaved, value); }
    public void SetUlongId(ulong id) { tno.Send(4, Target.AllSaved, id); }


    [RFC(0)] void RFC_SetDeviceId(string value) { m_deviceId = value; }
    [RFC(1)] void RFC_SetDeviceName(string value) { m_deviceName = value; }
    [RFC(3)] void RFC_SetDeviceModelLog(string value) { m_deviceDescription = value; }
    [RFC(2)] void RFC_SetDeviceAlias(string[] value) { m_deviceAliasNames = value; }
    [RFC(4)] void RFC_SetUlongId(ulong value) { m_uniqueId = value; }
  

    [ContextMenu("Manual Push Refresh")]
    public void ManualPushRefresh()
    {
        SetDeviceId(m_deviceId);
        SetDeviceName(m_deviceId);
        SetDeviceModelLog(m_deviceDescription);
        SetDeviceAlias(m_deviceAliasNames);
        SetUlongId(m_uniqueId);

    }
}
