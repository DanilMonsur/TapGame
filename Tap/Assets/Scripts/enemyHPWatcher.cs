using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class enemyHPWatcher : MonoBehaviour {

	private GameObject enemyHP;
	private GameObject enemy;

	// Use this for initialization
	[RuntimeInitializeOnLoadMethod]
	void Start () {
		enemyHP = GameObject.Find ("Overlay/EnemyHP");
		enemy = GameObject.Find ("Overlay/Enemy");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			int val = (int) enemyHP.GetComponent<Slider>().value;
			stream.Serialize (ref val);
		}
		else
		{
			int receivedVal = 0;
			stream.Serialize (ref receivedVal);
			enemyHP.GetComponent<Slider>().value = receivedVal;
		}
		
	}
}
