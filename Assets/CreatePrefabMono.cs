using System.Collections;
using System.Collections.Generic;
using TNet;
using UnityEngine;

/// <summary>
/// This script shows how to create objects dynamically over the network.
/// The same Instantiate call will work perfectly fine even if you're not currently connected.
/// This script is attached to the floor in Example 2.
/// </summary>

public class CreatePrefabMono : MonoBehaviour
{
	public string m_prefabName ;

	void Awake()
	{
		TNManager.Create(m_prefabName, Vector3.zero,Quaternion.identity,true);
	}

	
}
