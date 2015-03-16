using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RunGame : MonoBehaviour {

	private int enemyHPval;
	private Text enemyHPLabel;
	private GameObject enemyHP;
	private GameObject enemy;
	private GameObject nextEnemy;
	private int enemyAffinity;
	private int nextEnemyAffinity;
	private bool enemyIsAlive;
	private int affinityBonus;
	private int enemyInitHP;
	private int damage;

	[SerializeField] private Button Skill1Button = null;
	[SerializeField] private Button Skill2Button = null;
	[SerializeField] private Button Skill3Button = null;
	[SerializeField] private Transform enemyPrefab = null;
	[SerializeField] private Transform nextEnemyPrefab = null;



	// Use this for initialization


	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Server) 
		{
			GUI.Label (new Rect(10, 10, 300, 20), "Status: Connected as Server");
		}
		else if (Network.peerType == NetworkPeerType.Client) 
		{
			GUI.Label (new Rect(10, 10, 300, 20), "Status: Connected as Client");
		}
	}

	[RuntimeInitializeOnLoadMethod]
	void Start () {

		Application.runInBackground = true;

		Network.InitializeServer (3, 25001, false);

		if (Network.peerType == NetworkPeerType.Disconnected) 
		{
			Network.Connect ("127.0.0.1", 25001);
		}

		if(Network.peerType == NetworkPeerType.Server)
		{
			GameObject blah = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			blah = Instantiate(nextEnemyPrefab, Vector3.zero, Quaternion.identity) as GameObject;

			enemy = GameObject.Find ("Enemy(Clone)");
			nextEnemy = GameObject.Find ("NextEnemy(Clone)");

			enemy.transform.SetParent (GameObject.Find ("Overlay").transform);
			enemy.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			enemy.GetComponent<RectTransform>().localPosition = Vector3.zero;

			nextEnemy.transform.SetParent (GameObject.Find ("Overlay").transform);
			nextEnemy.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1f);
			nextEnemy.GetComponent<RectTransform>().anchoredPosition = new Vector3(-25, -25, 0);
		}


		affinityBonus = 5;

		Skill1Button.onClick.AddListener (() => {
			Skill (1);});

		Skill2Button.onClick.AddListener (() => {
			Skill (2);});

		Skill3Button.onClick.AddListener (() => {
			Skill (3);});

		enemyHP = GameObject.Find ("Overlay/EnemyHP");
		//enemy = GameObject.Find ("Overlay/Enemy");
		nextEnemy = GameObject.Find ("Overlay/NextEnemy");
		enemyHPLabel = GameObject.Find ("Overlay/EnemyHP/EnemyHPText").GetComponent<Text> ();

		enemyInitHP = 100;
		enemyHPval = enemyInitHP;
		enemyHP.GetComponent<Slider> ().maxValue = enemyHPval;
		enemyHP.GetComponent<Slider> ().value = enemyHPval;

		enemyIsAlive = true;
		GameObject.Find ("Overlay").GetComponent<NetworkView>().RPC("setEnemyAffinity", RPCMode.Server);

		enemy.GetComponent<Image> ().color = enemyAffinity == 0 ? Color.red : enemyAffinity == 1 ? Color.green : Color.blue;
		nextEnemy.GetComponent<Image>().color = nextEnemyAffinity == 0 ? Color.red : nextEnemyAffinity == 1 ? Color.green : Color.blue;

	}
	
	// Update is called once per frame
	void Update () {

		enemyIsAlive = enemyHPval > 0;
		enemy.transform.Rotate (Vector3.back);

		enemyHP.GetComponent<Slider> ().value = enemyHPval;
		enemyHPLabel.text = enemyHPval.ToString ();


		if (enemyIsAlive) {
			enemyHP.GetComponent<Slider> ().value = enemyHPval;
			enemyHPLabel.text = enemyHPval.ToString ();
		} 
		else
		{
			enemyHPval = enemyInitHP;
			enemy.GetComponent<Image> ().color = nextEnemy.GetComponent<Image> ().color;
			GameObject.Find ("Overlay").GetComponent<NetworkView>().RPC("updateEnemyAffinity", RPCMode.Server);
			nextEnemy.GetComponent<Image>().color = nextEnemyAffinity == 0 ? Color.red : nextEnemyAffinity == 1 ? Color.green : Color.blue;
		}

	}

	public void Skill(int affinity)
	{
		if ((affinity == 1 && enemyAffinity == 1) || (affinity == 2 && enemyAffinity == 0) || (affinity == 3 && enemyAffinity == 2))
		{
			enemyHPval -= affinityBonus;
		}
		else
		{
			enemyHPval -= 1;
		}

		GameObject.Find ("Overlay").GetComponent<NetworkView>().RPC("damageEnemy", RPCMode.Server, enemyHPval);
	}

	[RPC] void damageEnemy (int newHp)
	{
		enemyHPval = enemyHPval > 0 ? newHp : 0;
	}

	[RPC] void setEnemyAffinity()
	{
		enemyAffinity = (int)Random.Range(0, 10000) % 3;
		nextEnemyAffinity = (int)Random.Range(0, 10000) % 3;


	}

	[RPC] void updateEnemyAffinity()
	{
		enemyAffinity = nextEnemyAffinity;
		nextEnemyAffinity = (int)Random.Range(0, 10000) % 3;

	}
}
