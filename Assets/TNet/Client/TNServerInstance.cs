//-------------------------------------------------
//                    TNet 3
// Copyright © 2012-2020 Tasharen Entertainment Inc
//-------------------------------------------------

// Must also be defined in TNGameServer.cs
//#define SINGLE_THREADED

using UnityEngine;
using System.IO;
using System.Net;

namespace TNet
{
	/// <summary>
	/// Tasharen Network server tailored for Unity.
	/// </summary>

	public class TNServerInstance : MonoBehaviour
	{
		static TNServerInstance mInstance;

		[DoNotObfuscate] public enum Type
		{
			Lan,
			Udp,
			Tcp,
		}

		[DoNotObfuscate] public enum State
		{
			Inactive,
			Starting,
			Active,
		}

		GameServer mGame = new GameServer();
		LobbyServer mLobby;
		UPnP mUp = new UPnP();

		/// <summary>
		/// Instance access is internal only as all the functions are static for convenience purposes.
		/// </summary>

		static TNServerInstance instance
		{
			get
			{
				if (mInstance == null)
				{
					var go = new GameObject("_Server");
					mInstance = go.AddComponent<TNServerInstance>();
					DontDestroyOnLoad(go);
				}
				return mInstance;
			}
		}

		/// <summary>
		/// Custom packet receiving function, called from a worker thread.
		/// </summary>

		static public System.Action onReceivePackets { get { return (mInstance != null) ? mInstance.mGame.onReceivePackets : null; } set { if (mInstance != null) mInstance.mGame.onReceivePackets = value; } }

		/// <summary>
		/// Path to the admin file.
		/// </summary>

		static public string adminPath
		{
			get
			{
				if (!isActive) return GameServer.defaultAdminPath;
				return string.IsNullOrEmpty(mInstance.mGame.rootDirectory) ? mInstance.mGame.adminFilePath : Path.Combine(mInstance.mGame.rootDirectory, mInstance.mGame.adminFilePath);
			}
		}

		/// <summary>
		/// Private server -- doesn't allow other players.
		/// </summary>

		static public bool isPrivate { get { return (mInstance != null) && mInstance.mGame.isActive && !mInstance.mGame.isListening; } }

		/// <summary>
		/// Whether the server instance is currently active.
		/// </summary>

		static public bool isActive { get { return (mInstance != null) && mInstance.mGame.isActive; } }

		/// <summary>
		/// Whether the server is currently listening for incoming connections.
		/// </summary>

		static public bool isListening { get { return (mInstance != null) && mInstance.mGame.isListening; } set { if (mInstance != null) mInstance.mGame.isListening = value; } }

		/// <summary>
		/// Local server instance -- doesn't use sockets.
		/// </summary>

		static public bool isLocal { get { return (mInstance != null) && mInstance.mGame.isActive && (mInstance.mGame.localClient != null || !mInstance.mGame.isListening); } }

		/// <summary>
		/// Port used to listen for incoming TCP connections.
		/// </summary>

		static public int listeningPort { get { return (mInstance != null) ? mInstance.mGame.tcpPort : 0; } }

		/// <summary>
		/// Set your server's name.
		/// </summary>

		static public string serverName { get { return (mInstance != null) ? mInstance.mGame.name : null; } set { if (instance != null) mInstance.mGame.name = value; } }

		/// <summary>
		/// How many players are currently connected to the server.
		/// </summary>

		static public int playerCount { get { return (mInstance != null) ? mInstance.mGame.playerCount : 0; } }

		/// <summary>
		/// List of connected players.
		/// </summary>

		static public List<TcpPlayer> players { get { return (mInstance != null) ? mInstance.mGame.players : null; } }

		/// <summary>
		/// Active game server.
		/// </summary>

		static public GameServer game { get { return (mInstance != null) ? mInstance.mGame : null; } }

		/// <summary>
		/// Active lobby server.
		/// </summary>

		static public LobbyServer lobby { get { return (mInstance != null) ? mInstance.mLobby : null; } }

		/// <summary>
		/// Set the root directory for the server instance. Call this before Start().
		/// </summary>

		static public void SetRootDirectory (string path)
		{
			instance.mGame.rootDirectory = path;
		}

		/// <summary>
		/// Start dummy server instance without using sockets. Ideal for single-player.
		/// </summary>

		static public bool Start (string fileName)
		{
			return instance.StartLocal(fileName);
		}

		/// <summary>
		/// Start a local server instance listening to the specified port.
		/// </summary>

		static public bool Start (int tcpPort, bool openPort = true)
		{
			return instance.StartLocal(tcpPort, 0, null, 0, Type.Udp, openPort);
		}

		/// <summary>
		/// Start a local server instance listening to the specified port.
		/// </summary>

		static public bool Start (int tcpPort, int udpPort, bool openPort = true)
		{
			return instance.StartLocal(tcpPort, udpPort, null, 0, Type.Udp, openPort);
		}

		/// <summary>
		/// Start a local server instance listening to the specified port and loading the saved data from the specified file.
		/// </summary>

		static public bool Start (int tcpPort, int udpPort, string fileName, bool openPort = true)
		{
			return instance.StartLocal(tcpPort, udpPort, fileName, 0, Type.Udp, openPort);
		}

		[System.Obsolete("Use TNServerInstance.Start(tcpPort, udpPort, lobbyPort, fileName) instead")]
		static public bool Start (int tcpPort, int udpPort, string fileName, int lanBroadcastPort)
		{
			return instance.StartLocal(tcpPort, udpPort, fileName, lanBroadcastPort, Type.Udp, true);
		}

		/// <summary>
		/// Start a local game and lobby server instances.
		/// </summary>

		static public bool Start (int tcpPort, int udpPort, int lobbyPort, string fileName, bool openPort = true)
		{
			return instance.StartLocal(tcpPort, udpPort, fileName, lobbyPort, Type.Udp, openPort);
		}

		/// <summary>
		/// Start a local game and lobby server instances.
		/// </summary>

		static public bool Start (int tcpPort, int udpPort, int lobbyPort, string fileName, Type type, bool openPort = true)
		{
			return instance.StartLocal(tcpPort, udpPort, fileName, lobbyPort, type, openPort);
		}

		/// <summary>
		/// Start a local game server and connect to a remote lobby server.
		/// </summary>

		static public bool Start (int tcpPort, int udpPort, string fileName, Type type, IPEndPoint remoteLobby, bool openPort = true)
		{
			return instance.StartRemote(tcpPort, udpPort, fileName, remoteLobby, type, openPort);
		}

		/// <summary>
		/// Start a new server.
		/// </summary>

		bool StartLocal (string fileName)
		{
			// Ensure that everything has been stopped first
			if (mGame.isActive) Disconnect();

			// Start the game server
			if (mGame.Start())
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					mGame.Load(fileName);
#if SINGLE_THREADED
					mGame.Update();
#endif
				}
				return true;
			}

			// Something went wrong -- stop everything
			Disconnect();
			return false;
		}

		/// <summary>
		/// Start a new server.
		/// </summary>

		bool StartLocal (int tcpPort, int udpPort, string fileName, int lobbyPort, Type type, bool openPort)
		{
			// Ensure that everything has been stopped first
			if (mGame.isActive) Disconnect();

			// If there is a lobby port, we should set up the lobby server and/or link first.
			// Doing so will let us inform the lobby that we are starting a new server.

			if (lobbyPort > 0)
			{
				if (type == Type.Tcp) mLobby = new TcpLobbyServer();
				else mLobby = new UdpLobbyServer();

				// Start a local lobby server
				if (mLobby.Start(lobbyPort))
				{
					if (openPort)
					{
						if (type == Type.Tcp) mUp.OpenTCP(lobbyPort);
						else mUp.OpenUDP(lobbyPort);
					}
				}
				else
				{
					mLobby = null;
					return false;
				}

				// Create the local lobby link
				mGame.lobbyLink = new LobbyServerLink(mLobby);
			}

			// Start the game server
			if (mGame.Start(tcpPort, udpPort))
			{
				if (openPort)
				{
					mUp.OpenTCP(tcpPort);
					if (udpPort > 0) mUp.OpenUDP(udpPort);
				}
				if (!string.IsNullOrEmpty(fileName)) mGame.Load(fileName);
				return true;
			}

			// Something went wrong -- stop everything
			Disconnect();
			return false;
		}

		/// <summary>
		/// Start a new server.
		/// </summary>

		bool StartRemote (int tcpPort, int udpPort, string fileName, IPEndPoint remoteLobby, Type type, bool openPort)
		{
			if (mGame.isActive) Disconnect();

			if (remoteLobby != null && remoteLobby.Port > 0)
			{
				if (type == Type.Tcp)
				{
					mLobby = new TcpLobbyServer();
					mGame.lobbyLink = new TcpLobbyServerLink(remoteLobby);
				}
				else if (type == Type.Udp)
				{
					mLobby = new UdpLobbyServer();
					mGame.lobbyLink = new UdpLobbyServerLink(remoteLobby);
				}
				else
				{
					Debug.LogWarning("The remote lobby server type must be either UDP or TCP, not LAN");
				}
			}

			if (mGame.Start(tcpPort, udpPort))
			{
				if (openPort)
				{
					mUp.OpenTCP(tcpPort);
					mUp.OpenUDP(udpPort);
				}
				if (!string.IsNullOrEmpty(fileName)) mGame.Load(fileName);
				return true;
			}

			Disconnect();
			return false;
		}

		/// <summary>
		/// Stop the server.
		/// </summary>

		static public void Stop () { if (mInstance != null) mInstance.Disconnect(); }

		/// <summary>
		/// Make the server private by no longer accepting new connections.
		/// </summary>

		[System.Obsolete("Use 'isListening = false' instead")]
		static public void MakePrivate () { if (mInstance != null) mInstance.mGame.isListening = false; }

		/// <summary>
		/// Add a custom already-connected player.
		/// </summary>

		static public TcpPlayer AddPlayer (IConnection player) { return (mInstance != null) ? mInstance.mGame.AddPlayer(player) : null; }

		/// <summary>
		/// Remove the specified player.
		/// </summary>

		static public void RemovePlayer (IConnection player) { if (mInstance != null) mInstance.mGame.RemovePlayer(player); }

		/// <summary>
		/// Stop everything.
		/// </summary>

		void Disconnect ()
		{
			mGame.Stop();

			if (mLobby != null)
			{
				mLobby.Stop();
				mLobby = null;
			}
			mUp.Close();
		}

		/// <summary>
		/// Make sure that the servers are stopped when the server instance is destroyed.
		/// </summary>

		void OnDestroy ()
		{
			Disconnect();
			mUp.WaitForThreads();
		}

#if SINGLE_THREADED
		void Update () { if (mGame != null && mGame.isActive) mGame.Update(); }
#endif

		[System.Obsolete("Just call TNServerInstance.Stop() instead")]
		static public void Stop (string fileName) { Stop(); }

		[System.Obsolete("Calling this function is no longer necessary. The server will auto-save.")]
		static public void SaveTo (string fileName) { }
	}
}
