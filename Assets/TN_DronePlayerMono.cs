using System.Collections;
using System.Collections.Generic;
using TNet;
using UnityEngine;

[RequireComponent(typeof(TNObject))]
public class TN_DronePlayerMono : MonoBehaviour
{
	public static TN_DronePlayerMono Mine;
	[System.NonSerialized]
	public TNObject tno;
	public bool m_isMine;

	public string m_lastReceivedText;
	public byte[] m_lastReceivedByte;
	public Eloi.PrimitiveUnityEvent_String m_onTextReceived;
	public Eloi.PrimitiveUnityEvent_Bytes m_onBytesReceived;
	public bool m_sendToSelfToo;
	protected void Awake()
	{
		tno = GetComponent<TNObject>();
		m_isMine = tno.isMine;
		if (m_isMine) {
				Mine = this ;
		}
	}
	[ContextMenu("PushRandomText")]
	public void PushRandomText()
	{
		Eloi.E_UnityRandomUtility.GetRandomN2M(0, 500, out float r);
		PushTextToAllOnline("" + r);
	}
	[ContextMenu("PushRandomBytes")]
	public void PushRandomBytes()
	{
		byte[] value = new byte[2];
		Eloi.E_UnityRandomUtility.GetRandomN2M(0, 7, out int r);
		value[0] = (byte)r;
		Eloi.E_UnityRandomUtility.GetRandomN2M(0, 7, out  r);
		value[1] = (byte)r;
		PushBytesToAllOnline(value);
	}

	public void PushTextToAllOnline(string text)
	{
		if (!tno.isMine) return;
		if (tno.hasBeenDestroyed) return;
		if (m_sendToSelfToo)
			tno.SendQuickly("NotifyTextReceived", Target.AllSaved, text);
		else
		tno.SendQuickly("NotifyTextReceived", Target.OthersSaved, text);
	}
	public void PushBytesToAllOnline(byte[] value)
	{
		if (!tno.isMine) return;
		if (tno.hasBeenDestroyed) return;
		if(m_sendToSelfToo)
		tno.SendQuickly("NotifyByteReceived", Target.AllSaved, value);
		else
		tno.SendQuickly("NotifyByteReceived", Target.OthersSaved, value);
	}

	[RFC]
	protected void NotifyTextReceived(string value) { m_lastReceivedText = value; m_onTextReceived.Invoke(value); }
	[RFC]
	protected void NotifyByteReceived(byte[] value) { m_lastReceivedByte = value; m_onBytesReceived.Invoke(value); }


}
