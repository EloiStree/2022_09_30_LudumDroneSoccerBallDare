using System.Collections;
using System.Collections.Generic;
using System.Text;
using TNet;
using UnityEngine;

[RequireComponent(typeof(TNObject))]
public class TN_OMIComToAllMono:  TNBehaviour
{

	public bool m_sendToSelfToo;
	public Eloi.PrimitiveUnityEvent_String m_onTextReceived;
	public Eloi.PrimitiveUnityEvent_Bytes m_onBytesReceived;

	[Header("Debug Log")]
	public bool m_isMine;
	public string m_lastReceivedText;
	public byte[] m_lastReceivedByte;
	public static TN_OMIComToAllMono Mine;

	protected void Start()
	{
		m_isMine = tno.isMine;
		if (m_isMine)
		{
			Mine = this;
		}
	}
	[ContextMenu("PushRandomText")]
	public void PushRandomText()
	{
		Eloi.E_UnityRandomUtility.GetRandomN2M(0, 500, out float r);
		TCP_PushTextToAllOnline("D" + r);
	}

	[ContextMenu("PushRandomBytes")]
	public void PushRandomBytes()
	{
		byte[] value = new byte[2];
		Eloi.E_UnityRandomUtility.GetRandomN2M(0, 7, out int r);
		value[0] = (byte)r;
		Eloi.E_UnityRandomUtility.GetRandomN2M(0, 7, out r);
		value[1] = (byte)r;
		TCP_PushBytesToAllOnline(value);
	}
	public void TCP_PushTextToAllOnline(string text)
	{
		if (!tno.isMine) return;
		if (tno.hasBeenDestroyed) return;

		byte[] bytes = Encoding.Unicode.GetBytes(text);
		if (m_sendToSelfToo)
			tno.Send(7, Target.AllSaved, bytes);
		else
			tno.Send(7, Target.OthersSaved, bytes);
	}
	public void TCP_PushBytesToAllOnline(byte[] value)
	{
		if (!tno.isMine) return;
		if (tno.hasBeenDestroyed) return;
		if (m_sendToSelfToo)
			tno.Send(8, Target.AllSaved, value);
		else
			tno.Send(8, Target.OthersSaved, value);
	}

	public void UDP_PushTextToAllOnline(string text)
	{
		if (!tno.isMine) return;
		if (tno.hasBeenDestroyed) return;

		byte[] bytes = Encoding.Unicode.GetBytes(text);
		if (m_sendToSelfToo)
			tno.SendQuickly(7, Target.AllSaved, bytes);
		else
			tno.SendQuickly(7, Target.OthersSaved, bytes);
	}
	public void UDP_PushBytesToAllOnline(byte[] value)
	{
		if (!tno.isMine) return;
		if (tno.hasBeenDestroyed) return;
		if (m_sendToSelfToo)
			tno.SendQuickly(8, Target.AllSaved, value);
		else
			tno.SendQuickly(8, Target.OthersSaved, value);
	}
	[RFC(7)]
	protected void NotifyTextReceived(byte[] textAsBytes)
	{
		string value = Encoding.Unicode.GetString(textAsBytes);
		m_lastReceivedText = value;
		m_onTextReceived.Invoke(value); 
	}

	[RFC(8)]
	protected void NotifyByteReceived(byte[] value) {
		m_lastReceivedByte = value;
		m_onBytesReceived.Invoke(value); 
	}

}
