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

	[SerializeField] private Button Skill1Button = null;
	[SerializeField] private Button Skill2Button = null;
	[SerializeField] public Button Skill3Button = null;



	// Use this for initialization
	[RuntimeInitializeOnLoadMethod]
	void Start () {

		Application.runInBackground = true;

		affinityBonus = 5;

		Skill1Button = GameObject.Find ("Overlay/Skill1").GetComponent<Button>();
		Skill2Button = GameObject.Find ("Overlay/Skill2").GetComponent<Button>();
		Skill3Button = GameObject.Find ("Overlay/Skill3").GetComponent<Button>();

		Skill1Button.onClick.AddListener (() => {
			Skill1 ();});

		Skill2Button.onClick.AddListener (() => {
			Skill2 ();});

		Skill3Button.onClick.AddListener (() => {
			Skill3 ();});

		enemyHP = GameObject.Find ("Overlay/EnemyHP");
		enemy = GameObject.Find ("Overlay/Enemy");
		nextEnemy = GameObject.Find ("Overlay/NextEnemy");
		enemyHPLabel = GameObject.Find ("Overlay/EnemyHP/EnemyHPText").GetComponent<Text> ();

		enemyInitHP = 100;
		enemyHPval = enemyInitHP;
		enemyHP.GetComponent<Slider> ().maxValue = enemyHPval;
		enemyHP.GetComponent<Slider> ().value = enemyHPval;

		enemyIsAlive = true;
		setEnemyAffinity ();

	}
	
	// Update is called once per frame
	void Update () {

		enemyIsAlive = enemyHPval > 0;
		enemy.transform.Rotate (Vector3.back);

		if(Network.isServer)
		{
			if (enemyIsAlive) {
			enemyHP.GetComponent<Slider> ().value = enemyHPval;
			enemyHPLabel.text = enemyHPval.ToString ();
			} 
			else
			{
				enemyHPval = enemyInitHP;
				updateEnemyAffinity();
			}
		}
	}



	public void Skill1()
	{
		enemyHPval -= enemyAffinity == 1 ? affinityBonus : 1;
	}

	public void Skill2()
	{
		enemyHPval -= enemyAffinity == 0 ? affinityBonus : 1;
	}

	public void Skill3()
	{
		enemyHPval -= enemyAffinity == 2 ? affinityBonus : 1;
	}

	void setEnemyAffinity()
	{
		enemyAffinity = (int)Random.Range(0, 10000) % 3;
		nextEnemyAffinity = (int)Random.Range(0, 10000) % 3;

		enemy.GetComponent<Image> ().color = enemyAffinity == 0 ? Color.red : enemyAffinity == 1 ? Color.green : Color.blue;
		nextEnemy.GetComponent<Image>().color = nextEnemyAffinity == 0 ? Color.red : nextEnemyAffinity == 1 ? Color.green : Color.blue;
	}

	void updateEnemyAffinity()
	{
		enemyAffinity = nextEnemyAffinity;
		enemy.GetComponent<Image> ().color = nextEnemy.GetComponent<Image> ().color;

		nextEnemyAffinity = (int)Random.Range(0, 10000) % 3;
		nextEnemy.GetComponent<Image>().color = nextEnemyAffinity == 0 ? Color.red : nextEnemyAffinity == 1 ? Color.green : Color.blue;
	}
}
