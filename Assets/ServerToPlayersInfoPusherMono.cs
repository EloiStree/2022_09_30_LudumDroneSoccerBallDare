using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerToPlayersInfoPusherMono : MonoBehaviour
{
    
    public void PushBytesToPlayers(byte[] obj)
    {
        ServerToPlayersInfo.I.PushToPlayers(obj);
    }

    public void PushTextToPlayers(string obj)
    {
        ServerToPlayersInfo.I.PushToPlayers(obj);
    }

    [ContextMenu("SendRandomChar")]
    public void SendRandomChar() {
        Eloi.E_UnityRandomUtility.GetRandomOf("abcdefghijklmnopqrstuvwxyz", out char c);
        PushTextToPlayers("" + c);
    }
}