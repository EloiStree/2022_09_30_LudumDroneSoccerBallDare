using System.Collections;
using System.Collections.Generic;
using TNet;
using UnityEngine;

[RequireComponent(typeof(TNObject))]
public class TN_DroneCompressedPositionList : TNBehaviour
{
	public Transform m_coordinateOrigin;
	public DroneInGameToTrackGroup m_trackedDrone;
	public DroneInGameGroup m_droneCompressPosition;
	public DroneInGameGroup m_droneCompressPositionReceived;
	public bool m_isConsiderServer;

	public void Update()
	{
		if (m_isConsiderServer) { 
		 }
		if (tno.isMine ) { 
			if (m_droneCompressPosition.m_droneInGame.Length != m_trackedDrone.m_droneInGame.Length) {
				m_droneCompressPosition.m_droneInGame = new DroneInGame[m_trackedDrone.m_droneInGame.Length];
			}
			for (int i = 0; i < m_droneCompressPosition.m_droneInGame.Length; i++)
			{
				m_trackedDrone.m_droneInGame[i].m_droneId = m_trackedDrone.m_droneInGame[i].m_droneId;
				CompressPositionSmallAreaUtility.CompressInRef(
					m_coordinateOrigin, 
					m_trackedDrone.m_droneInGame[i].m_droneRoot,
					ref m_droneCompressPosition.m_droneInGame[i].m_compressLocalPosition
					);
			}
		}
		//tno.SendQuickly(0, Target.AllSaved, m_droneCompressPosition);
		tno.SendQuickly("SetReceiverSize", Target.AllSaved, m_droneCompressPosition.m_droneInGame.Length);
		for (int i = 0; i < m_droneCompressPosition.m_droneInGame.Length; i++)
        {
			tno.SendQuickly("SetWithDroneCompressedPosition", Target.AllSaved,
		    m_droneCompressPosition.m_droneInGame[i].m_compressLocalPosition.m_position,
			m_droneCompressPosition.m_droneInGame[i].m_compressLocalPosition.m_rotation);
		}
	}

	[RFC(1)]
	void SetWithDroneCompressedPosition(int index,ushort id,  ulong positions, ulong rotations)
	{
		m_droneCompressPositionReceived.m_droneInGame[index].m_droneId=id;
		m_droneCompressPositionReceived.m_droneInGame[index].m_compressLocalPosition.m_position = positions;
		m_droneCompressPositionReceived.m_droneInGame[index].m_compressLocalPosition.m_rotation = rotations;
	}
	[RFC(2)]
	void SetReceiverSize(int size)
	{
		if (m_droneCompressPositionReceived.m_droneInGame.Length != size) {
			m_droneCompressPositionReceived.m_droneInGame = new DroneInGame[size];
		}
	}


}


[System.Serializable]
public struct DroneInGameToTrackGroup
{
	public DroneInGameToTrack[] m_droneInGame;
}
[System.Serializable]
public struct DroneInGameGroup
{
	public DroneInGame[] m_droneInGame;
}

[System.Serializable]
public struct DroneInGameToTrack
{
	public ushort m_droneId;
	public Transform m_droneRoot;
}
[System.Serializable]
public struct DroneInGame
{
	public ushort m_droneId;
	public CompressPositionSmallArea m_compressLocalPosition;
}
