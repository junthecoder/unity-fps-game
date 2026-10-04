// Historical Unity networking experiment; the main menu does not expose a completed multiplayer mode.

using UnityEngine;
using System.Collections;

public class NetworkTest : MonoBehaviour {

	enum State
	{
		Menu,
		JoiningServer,
		StartingServer,
		Server,
		Playing
	}

	State state = State.Menu;
	string serverAddress = "127.0.0.1";
	const string demoPassword = "DEMO_ONLY_NOT_A_REAL_CREDENTIAL";
	int maxConnections = 32;
	int port = 25000;
	static string log;

	public GameObject playerPrefab;

	void Start() {
		// setup proxy
		//Network.proxyIP = "1.1.1.1";
		//Network.proxyPort = 1111;
		//Network.useProxy = true;
	}
	
	void Update() {
	
	}

	void OnServerInitialized()
	{
		state = State.Server;
		Log("Server started...");
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		Log("Player connected... " + player.ToString());
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Log("Player disconnected... " + player.ToString());
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	void OnConnectedToServer()
	{
		state = State.Playing;
		Log("Successfully connected to server.");

		Destroy(Camera.main.gameObject);
		Network.Instantiate(playerPrefab, new Vector3(0, 2, 0), Quaternion.identity, 0);
	}

	void OnDisconnectedFromServer(NetworkDisconnection mode)
	{
		state = State.Menu;
		Log("Disconnected from server. " + mode);
	}

	void OnFailedToConnect(NetworkConnectionError error)
	{
		state = State.Menu;
		Log("Connection failed. " + error);
	}

	public static void Log(string text)
	{
		Debug.Log(text);
		log += text + "\n";
	}

	void OnGUI()
	{
		GUILayout.Label(log);

		if (state == State.Menu)
		{
			if (GUILayout.Button("Join Game"))
			{
				state = State.JoiningServer;

				Log(string.Format("Joining server {0}:{1}...\n", serverAddress, port));
				if (Network.useProxy)
					Log(string.Format("Using proxy {0}:{1}", Network.proxyIP, Network.proxyPort));
				else
					Log("No proxy");

				Network.Connect(serverAddress, port, demoPassword);
			}
			if (GUILayout.Button("Host Game"))
			{
				state = State.StartingServer;
				Log("Starting server...\n");
				Network.incomingPassword = demoPassword;
				Network.InitializeServer(maxConnections, port, !Network.HavePublicAddress());
			}
		}
	}
}
