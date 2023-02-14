using System;
using System.Collections;
using System.Collections.Generic;
using TNet;
using UnityEngine;



public class ServerToPlayersInfo {

	public static ServerToPlayersInfoMono I;
	public static void PushToPlayers(string textToSend)
	{
		if (I) I.PushToPlayers(textToSend);
	}
	public static void PushToPlayers(byte [] byteToSend)
	{
		if (I) I.PushToPlayers(byteToSend);
	}
	public static Action<string> m_textReceivedForPlayer;
	public static Action<byte[]> m_bytesReceivedForPlayer;
}


[RequireComponent(typeof(TNObject))]
public class ServerToPlayersInfoMono : TNBehaviour
{
	public bool m_serverToo=true;

	
	public void PushToPlayers(string textToSend)
	{
		if (m_serverToo)
			tno.SendQuickly("NotifyPlayerTextReceived", Target.AllSaved, textToSend);
		else
			tno.SendQuickly("NotifyPlayerTextReceived", Target.OthersSaved, textToSend);
	}
	public void PushToPlayers(byte[] byteToSend)
	{
		if (m_serverToo)
			tno.SendQuickly("NotifyPlayerBytesReceived", Target.AllSaved, byteToSend);
		else
			tno.SendQuickly("NotifyPlayerBytesReceived", Target.OthersSaved, byteToSend);
	}
	public void Start()
	{
		ServerToPlayersInfo.I = this;
	}
	public void OnEnable()
    {
		ServerToPlayersInfo.I = this;
    }
	[RFC(0)] void NotifyPlayerTextReceived(string value) { ServerToPlayersInfo.m_textReceivedForPlayer.Invoke(value); }
	[RFC(1)] void NotifyPlayerBytesReceived(byte[] value) { ServerToPlayersInfo.m_bytesReceivedForPlayer.Invoke(value); }

}