using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerToPlayersInfoListenerMono : MonoBehaviour
{
    public Eloi.PrimitiveUnityEvent_String m_receivedText;
    public Eloi.PrimitiveUnityEvent_Bytes m_receivedBytes;
    private void Awake()
    {
        ServerToPlayersInfo.m_textReceivedForPlayer += PushReceivedText;
        ServerToPlayersInfo.m_bytesReceivedForPlayer += PushBytesReceived;
    }

    private void PushBytesReceived(byte[] obj)
    {
        m_receivedBytes.Invoke(obj);
    }

    private void PushReceivedText(string obj)
    {
        m_receivedText.Invoke(obj);
    }
}

