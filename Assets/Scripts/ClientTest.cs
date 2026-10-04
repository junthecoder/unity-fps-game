// Historical Unity networking experiment; not a completed multiplayer implementation.

using UnityEngine;
using System.Collections;

public class ClientTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var error = Network.Connect("127.0.0.1", 25000, "abcd");
		Debug.Log(error);
		

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	void PrintText(string text)
	{
		Debug.Log(text);
	}

	void OnConnectedToServer()
	{
		Debug.Log("Connected to server");
		networkView.RPC("PrintText", RPCMode.Others, new string[] {"test"});
	}
}
