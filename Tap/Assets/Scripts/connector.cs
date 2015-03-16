using UnityEngine;
using System.Collections;

public class connector : MonoBehaviour {

	public string connectionIp = "127.0.0.1";
	public int connectionPort = 25001;

	void OnGUI()
	{
		Application.runInBackground = true;



		if (Network.peerType == NetworkPeerType.Disconnected) 
		{
			GUI.Label (new Rect(10, 10, 200, 20), "Status: Disconnected");

			if (GUI.Button (new Rect(10, 30, 120, 20), "Client Connect"))
			{
				Network.Connect (connectionIp, connectionPort);
			}
		}
		else if (Network.peerType == NetworkPeerType.Client)
		{
			GUI.Label (new Rect(10, 10, 300, 20), "Status: Connected as Client");

			if (GUI.Button (new Rect(10, 30, 120, 20), "Disconnect"))
			{
				Network.Disconnect (200);
			}
		}
		else if (Network.peerType == NetworkPeerType.Server)
		{
			GUI.Label (new Rect(10, 10, 300, 20), "Status: Connected as Server");

			if (GUI.Button (new Rect(10, 30, 120, 20), "Disconnect"))
			{
				Network.Disconnect (200);
			}
		}
	}

}
