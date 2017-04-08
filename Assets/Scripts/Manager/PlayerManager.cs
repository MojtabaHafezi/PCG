using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance = null;
	//Player stats related components
	public int Health{ get; set; }

	public int CurrentHealth{ get; set; }


	public int Attack { get; set; }

	public int Defense{ get; set; }

	public int Gold { get; set; }

	public int Element{ get; set; }


	//Equipment/Inventory related components
	public int attackMod, defenseMod;
	private Dictionary<String, Item> inventory;

	//Messages for player with auto close
	public GameObject messageWindow;
	//Timing
	private float closeDelay;
	private float delay;
	private bool closed;


	// Battle Window Setup
	public GameObject battleWindow;
	public GameObject actionsWindow;
	public GameObject hudWindow;
	public bool isFighting;

	//Enemy
	public Enemy enemy;
	Slider enemySlider;
	Slider playerSlider;

	public GameObject GameOverScreen;
	public bool IsDead;
	public bool IsEnemyFleeing;


	//Artifical Neuronal Network
	public ControlNetwork ANN = new ControlNetwork ();
	public double[,] Trainingsset = new double[3, 10] {
		//Health, Player Health, Difference Att, Difference Def, Class difference (3 input) ||out: att, special, flee
		{ 1.0, 1.0, 0.1, 0.1, 0, 0, 1, 0, 1, 0 },
		{ 0.5, 1.0, 0.1, 0.1, 1, 0, 0, 0, 0, 1 },
		{ 1.0, 1.0, 0.1, 0.1, 1, 0, 0, 0, 0, 1 }

	};


	public void InititalizeANN ()
	{
		ANN.Initialize (7, 3, 5);
		ANN.SetLearningRate (0.3);
		ANN.SetMomentum (true, 0.8);
	}

	public void TrainANN ()
	{
		Debug.Log (Trainingsset.Length + "Length");
		double error = 1;
		int counter = 0;
		string message = "Pre Training";
		message += ANN.ToStringData ();
		Debug.Log (message);
		while ((error > 0.05) && (counter < 50000)) {
			error = 0;
			counter++;
			for (int i = 0; i < 3; i++) {
				ANN.GiveInput (0, Trainingsset [i, 0]);
				ANN.GiveInput (1, Trainingsset [i, 1]);
				ANN.GiveInput (2, Trainingsset [i, 2]);
				ANN.GiveInput (3, Trainingsset [i, 3]);
				ANN.GiveInput (4, Trainingsset [i, 4]);
				ANN.GiveInput (5, Trainingsset [i, 5]);
				ANN.GiveInput (6, Trainingsset [i, 6]);
		
				ANN.DesiredOutput (0, Trainingsset [i, 7]);
				ANN.DesiredOutput (1, Trainingsset [i, 8]);
				ANN.DesiredOutput (2, Trainingsset [i, 9]);

				ANN.FeedForward ();
				error += ANN.CalculateError ();
				ANN.BackPropogate ();

			}
			error = error / 3;
		}
		//Debug.Log (error);
		Debug.Log (ANN.ToStringData ());

	}

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	
		inventory = new Dictionary<String, Item> ();
		closeDelay = 2.0f;
		messageWindow.SetActive (false);
		battleWindow.SetActive (false);
		actionsWindow.SetActive (false);
		hudWindow.SetActive (false);
		closed = true;
		isFighting = IsDead = IsEnemyFleeing = false;

		Attack = UnityEngine.Random.Range (10, 12);
		Defense = UnityEngine.Random.Range (10, 12);
		Element = Random.Range (0, 3);
		Gold = 0;
		Health = CurrentHealth = 100;
		InititalizeANN ();
		TrainANN ();
	}

	void OnDisable ()
	{
		messageWindow.SetActive (false);
		battleWindow.SetActive (false);
		actionsWindow.SetActive (false);
		hudWindow.SetActive (false);
	}

	void OnEnable ()
	{
		//inventory = new Dictionary<String, Item> ();
		closeDelay = 2.0f;
		messageWindow.SetActive (false);
		battleWindow.SetActive (false);
		actionsWindow.SetActive (false);
		hudWindow.SetActive (true);
		closed = true;
		isFighting = IsDead = false;

		//Attack = UnityEngine.Random.Range (10, 12);
		//Defense = UnityEngine.Random.Range (10, 12);
		//Gold = 0;
		//Health = CurrentHealth = 100;
	}

	public  void UpdateInventory (Item item)
	{
		
		String message = "Found ";
		Item getItem = null;
		switch (item.type) {
		case itemType.glove:
			message += "gloves";
			if (inventory.ContainsKey ("glove")) {
				getItem = inventory ["glove"];
				//inventory.TryGetValue ("glove", out getItem);
				if ((getItem.attackMod + getItem.defenseMod) < (item.attackMod + item.defenseMod)) {
					inventory ["glove"] = item;
				}
			} else {
				inventory ["glove"] = item;
			}
			break;
		case itemType.boot:
			message += "boots";
			if (inventory.ContainsKey ("boot")) {
				getItem = inventory ["boot"];
				if ((getItem.attackMod + getItem.defenseMod) < (item.attackMod + item.defenseMod))
					inventory ["boot"] = item;
			} else {
				inventory ["boot"] = item;
			}
			break;
		case itemType.weapon:
			message += "a weapon";
			if (inventory.ContainsKey ("weapon")) {
				getItem = inventory ["weapon"];
				if ((getItem.attackMod + getItem.defenseMod) < (item.attackMod + item.defenseMod))
					inventory ["weapon"] = item;
			} else {
				inventory ["weapon"] = item;
			}
			break;
		}
		message += string.Format (" with an attack of {0} and a defense of {1}", item.attackMod, item.defenseMod);
		ShowMessage (message, 2.0f);
		ToLog (message);

		attackMod = 0;
		defenseMod = 0;
		foreach (KeyValuePair<String, Item> gear in inventory) {
			attackMod += gear.Value.attackMod;
			defenseMod += gear.Value.defenseMod;
		}

	}

	public void ShowMessage (string message, float delayTime)
	{
		messageWindow.SetActive (false);
		messageWindow.GetComponentInChildren<Text> ().text = message;
		messageWindow.SetActive (true);
		messageWindow.GetComponentInChildren<Text> ().text = message;

		closeDelay = delayTime;
		delay = 0;
		closed = false;
	}

	public void ShowActions ()
	{
		if (!IsDead)
			actionsWindow.SetActive (true);
	}

	public void HideActions ()
	{
		actionsWindow.SetActive (false);
	}

	public void StartBattle ()
	{
		battleWindow.SetActive (true);
		hudWindow.SetActive (false);
		HideActions ();
		ShowMessage ("Wild monster encountered!", 1.0f);
		enemy = new Enemy ();
		//CurrentHealth = Health;
		playerSlider = GameObject.FindGameObjectWithTag ("PlayerHealth").gameObject.GetComponent<Slider> ();
		enemySlider = GameObject.FindGameObjectWithTag ("EnemyHealth").gameObject.GetComponent<Slider> ();
		UpdateStats ();
		Invoke ("ShowActions", 1);
		IsEnemyFleeing = false;
		isFighting = true;

	}

	public void CloseBattle ()
	{
		HideActions ();
		hudWindow.SetActive (true);
		battleWindow.SetActive (false);
		enemy = null;
		isFighting = IsEnemyFleeing = false;
		if (IsDead) {
			isFighting = true;
			Invoke ("GameOver", 1);
		}
	

	}

	private void Update ()
	{
		if (!closed) {
			// add the time of the frame
			delay += Time.deltaTime;
			if (delay >= closeDelay) {
				closed = true;
				messageWindow.SetActive (false);
				delay = 0;
			}
		}


		if (CurrentHealth <= 0 && !IsDead) {
			isFighting = true;
			IsDead = true;
			ShowMessage ("You died...", 1);
			Invoke ("CloseBattle", 2);
		}


	}

	public void GameOver ()
	{
		GameOverScreen.GetComponent<PCGDungeonScreen> ().ToGameOver ();
	}

	public void Action (int id)
	{
		HideActions ();
		DealDamages (id);
		Invoke ("ShowActions", 2);


	}

	public void PlayerFlees ()
	{
		string message = "You have chosen to flee. ";
		int enemyAttack = (int)UnityEngine.Random.Range (0, 2);

		bool weaken = true;

		message += " \nYour enemy has chosen: ";
		switch (enemyAttack) {
		case 0:
			message += "Fast Attack.";
			break;
		case 1:
			message += "Normal Attack.";
			break;
		case 2:
			message += "Power Attack.";
			break;
		default:
			break;
		}


		int enemyDamage = enemy.Attack;
		int enemyDefense = enemy.Defense;


		//Player Gets Damage
		int damage = enemyDamage;
		if (weaken)
			damage = Mathf.RoundToInt (damage * 1.5f);
		int defense = (int)Math.Round ((double)(defenseMod + Defense) * 0.25);
		Mathf.Max (CurrentHealth -= (damage - defense), 0);
		message += string.Format ("\nDamage received: {0}", (damage - defense));

		if (!IsDead)
			message += "\n You got away!";
		UpdateStats ();
		ShowMessage (message, 1);
		ToLog (message);
		Invoke ("CloseBattle", 1);
		
	}

	// Deals damages to both characters, decides on the boost and weakness
	// 0= fast, 1 = norm, 2 = power; norm > fast > power > norm
	public void DealDamages (int id)
	{
		string message = "You have chosen: ";
		int enemyAttack = (int)UnityEngine.Random.Range (0, 4);
		bool boost = false;
		bool weaken = false;
		bool element = false;
		bool enemyElement = false;

		switch (id) {
		case 0: 
			message += "Fast Attack.";
			if (enemyAttack == 1)
				weaken = true;
			if (enemyAttack == 2)
				boost = true;
			break;
		case 1: 
			message += "Normal Attack.";
			if (enemyAttack == 0)
				boost = true;
			if (enemyAttack == 2)
				weaken = true;
			break;
		case 2:
			message += "Power Attack.";
			if (enemyAttack == 0)
				weaken = true;
			if (enemyAttack == 1)
				boost = true;
			break;
		//Special attack
		case 3:
			message += "Special Attack: " + GetElement (Element);
			if (Element == 0) {
				if (enemy.Element == 2) {
					element = true;
					boost = true;
				}

			} else if (Element == 1) {
				if (enemy.Element == 0) {
					element = true;
					boost = true;
				}
					
			} else if (Element == 3) {
				if (enemy.Element == 1) {
					element = false;
					boost = true;
				}
			}
			break;
		default:
			break;
		}
		if (Random.Range (0, 15) == 1) {
			EnemyFlees ();
			message += "\n Your enemy is trying to flee.";
			boost = true;
		}
			
		if (!IsEnemyFleeing) {
			message += " \nYour enemy has chosen: ";
			switch (enemyAttack) {
			case 0:
				message += "Fast Attack.";
				break;
			case 1:
				message += "Normal Attack.";
				break;
			case 2:
				message += "Power Attack.";
				break;
			case 3:
				message += "Special Attack " + GetElement (enemy.Element);
				switch (enemy.Element) {
				case 0:
					if (Element == 2) {
						weaken = true;
						enemyElement = true;
					}

					break;
				case 1:
					if (Element == 0) {
						weaken = true;
						enemyElement = true;
					}
					break;
				case 2: 
					if (Element == 1) {
						weaken = true;
						enemyElement = true;
					}
					break;
				default:
					break;
				}
				break;
			default:
				break;
			}
		}
	

		int enemyDamage = enemy.Attack;
		int enemyDefense = enemy.Defense;
		//Enemy Gets Damage
		int damage = Attack + attackMod;
		if (boost)
			damage = Mathf.RoundToInt (damage * 1.25f);
		if (element)
			damage += Random.Range (3, 8);
		int defense = Mathf.RoundToInt (enemyDefense * 0.45f);
		if (element)
			defense += Random.Range (3, 8);
		Mathf.Max (enemy.CurrentHealth -= (damage - defense), 0);
		message += string.Format ("\nDamage dealt: {0}", (damage - defense));

		if (!IsEnemyFleeing) {
			//Player Gets Damage
			damage = enemyDamage;
			if (weaken)
				damage = Mathf.RoundToInt (damage * 1.25f);
			if (enemyElement)
				damage += Random.Range (5, 15);
			defense = (int)Math.Round ((double)(defenseMod + Defense) * 0.45f);
			if (enemyElement)
				defense += Random.Range (3, 8);
			Mathf.Max (CurrentHealth -= (damage - defense), 0);
			message += string.Format ("\nDamage received: {0}", (damage - defense));

		}

		if (!IsDead && enemy.CurrentHealth <= 0) {
			
			message += string.Format ("\nYou won! You earned {0} gold!", enemy.Gold);
			ToLog (message);
			ShowMessage (message, 2.5f);
			Gold += enemy.Gold;
			CurrentHealth = Health;
			UpdateStats ();
			Invoke ("CloseBattle", 1);	
		} else if (IsEnemyFleeing) {
			message += "\n The enemy got away!";
			CurrentHealth = Health;
			UpdateStats ();
			ToLog (message);
			ShowMessage (message, 2);
			Invoke ("CloseBattle", 1);	
		} else {
			UpdateStats ();
			ToLog (message);
			ShowMessage (message, 2);

		}
	}

	public void EnemyFlees ()
	{
		IsEnemyFleeing = true;
	}

		
	private void UpdateStats ()
	{

		enemySlider.maxValue = enemy.Health;
		enemySlider.value = enemy.CurrentHealth;
		playerSlider.maxValue = Health;
		playerSlider.value = CurrentHealth;
		battleWindow.GetComponent<GUIAttributes> ().UpdateGUI ();
	}

	public string GetElement (int element)
	{
		switch (element) {
		case 0: 
			return "Water";

		case 1: 
			return "Earth";

		case 2: 
			return "Fire";

		default:
			return "Nothing";

		}
	}

	public void ToLog (string message)
	{
		Debug.Log (message);
	}


}
