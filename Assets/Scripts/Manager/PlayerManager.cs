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

	public bool element;

	//Equipment/Inventory related components
	public int attackMod, defenseMod;
	private Dictionary<String, Item> inventory;
	public static float DefenseFactor = 0.25f;

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
	public GameObject Log;
	public Text logText;
	public bool IsFighting;

	//Enemy
	public Enemy enemy;
	Slider enemySlider;
	Slider playerSlider;
	public bool enemyElement;

	public GameObject GameOverScreen;
	public bool IsDead;
	public bool IsEnemyFleeing;

	//CONSTANTS
	public const double GOLDMIN = 0;
	public const double GOLDMAX = 2000;
	public const double HEALTHMIN = 0;
	public const double HEALTHMAX = 200;
	public const double DIFFMIN = -100;
	public const double DIFFMAX = 100;
	public int playerStartHealth;
	public int enemyStartHealth;
	public int playerStartGold;

	//Artifical Neuronal Network
	public ControlNetwork ANN = new ControlNetwork ();
	//FOR DEMO
	//public  const int TESTNUMBER = 40;
	public  const int TESTNUMBER = 3;

	//number of input and output neurons (not hidden!)
	public const int TOTALNEURONS = 11;
	public const int INPUTNEURONS = 8;
	public const int OUTPUTNEURONS = 3;
	public const int HIDDENNEURONS = 10;
	public const bool CONTROL = true;
	public double[] InputForNeurons;
	//Class difference has 3 patterns, 001, 010, 100 - from good, normal and bad

	//Training data: when the enemy is too weak to begin with -> flee
	//if the player is weak -> special
	//if not too much difference in skills -> fight
	//when player is somewhat weak -> special

	//FOR DEMO
	public double[,] Trainingsset = new double[TESTNUMBER, TOTALNEURONS] {
		//Health, Player Health, Difference Att, Difference Def, Class difference (3 input), Gold of player ||out: att, special, flee00
		{ 1.0, 1.0, 0.5, 0.5, 0, 1, 0, 0.05, 1, 0, 0 },
		{ 1.0, 1.0, 0.5, 0.5, 1, 0, 0, 0.05, 0, 0, 1 },
		{ 1.0, 1.0, 0.5, 0.5, 0, 0, 1, 0.05, 0, 1, 0 },
		//{ 1.0, 1.0, 0.25, 0.25, 0, 1, 0, 0.05, 0, 0, 1 },
		//{ 1.0, 1.0, 0.6, 0.6, 0, 1, 0, 0.05, 1, 0, 0 }
	};
	/*
	public double[,] Trainingsset = new double[TESTNUMBER, TOTALNEURONS] {
		//Health, Player Health, Difference Att, Difference Def, Class difference (3 input), Gold of player ||out: att, special, flee
		{ 1.0, 1.0, 0.1, 0.1, 0, 0, 1, 0.02, 0, 1, 0 },
		{ 0.5, 1.0, 0.1, 0.1, 1, 0, 0, 0.02, 0, 0, 1 },
		{ 1.0, 1.0, 0.1, 0.1, 1, 0, 0, 0.02, 0, 0, 1 },
		{ 0.6, 0.5, 0.575, 0.525, 0, 1, 0, 0.02, 1, 0, 0 },
		{ 0.9, 0.5, 0.425, 0.375, 0, 1, 0, 0.02, 1, 0, 0 },
		{ 0.5, 0.75, 0.625, 0.625, 0, 1, 0, 0.02, 1, 0, 0 },
		{ 0.25, 0.6, 0.4, 0.375, 1, 0, 0, 0.02, 1, 0, 0 },
		{ 0.25, 0.25, 0.4, 0.375, 1, 0, 0, 0.02, 1, 0, 0 },
		{ 0.25, 0.25, 0.4, 0.375, 0, 0, 1, 0.02, 0, 1, 0 },
		{ 0.25, 0.25, 0.4, 0.375, 0, 1, 0, 0.02, 1, 0, 0 },
		{ 0.75, 0.5, 0.6, 0.6, 1, 0, 0, 0.02, 1, 0, 0 },
		{ 0.75, 0.5, 0.6, 0.6, 0, 1, 0, 0.02, 1, 0, 0 },
		{ 0.75, 0.5, 0.6, 0.6, 0, 0, 1, 0.02, 0, 1, 0 },
		{ 0.5, 0.5, 0.6, 0.6, 0, 0, 1, 0.02, 0, 1, 0 },
		{ 0.25, 0.5, 0.6, 0.6, 0, 0, 1, 0.02, 0, 1, 0 },
		{ 0.5, 0.5, 0.5, 0.5, 0, 0, 1, 0.02, 0, 1, 0 },
		{ 0.5, 0.5, 0.5, 0.5, 0, 1, 0, 0.02, 1, 0, 0 },
		{ 0.5, 0.5, 0.5, 0.5, 1, 0, 0, 0.02, 1, 0, 0 },
		{ 0.5, 0.5, 0.35, 0.35, 1, 0, 0, 0.02, 0, 0, 1 },
		{ 0.6, 0.2, 0.55, 0.55, 0, 1, 0, 0.02, 0, 1, 0 },
		{ 0.6, 0.2, 0.45, 0.45, 0, 0, 1, 0.02, 0, 1, 0 },
		{ 0.6, 0.2, 0.65, 0.65, 1, 0, 0, 0.02, 0, 1, 0 },
		{ 1.0, 1.0, 0.25, 0.25, 0, 0, 1, 0.1, 0, 1, 0 },
		{ 1.0, 1.0, 0.25, 0.25, 1, 0, 0, 0.5, 1, 0, 0 },
		{ 1.0, 1.0, 0.15, 0.15, 1, 0, 0, 0.65, 1, 0, 0 },
		{ 1.0, 1.0, 0.15, 0.15, 0, 1, 0, 0.65, 1, 0, 0 },
		{ 1.0, .45, 0.15, 0.15, 0, 1, 0, 0.65, 0, 1, 0 },
		{ .50, .45, 0.45, 0.45, 0, 1, 0, 0.45, 0, 1, 0 },
		{ .50, .45, 0.45, 0.45, 0, 1, 0, 0.45, 0, 1, 0 },
		{ .250, .45, 0.65, 0.65, 0, 1, 0, 0.35, 1, 0, 0 },
		{ .50, .8, 0.45, 0.45, 1, 0, 0, 0.02, 0, 0, 1 },
		{ .750, .65, 0.35, 0.35, 1, 0, 0, 0.25, 1, 0, 0 },
		{ .750, .25, 0.3, 0.3, 1, 0, 0, 0.55, 0, 1, 0 },
		{ .80, .95, 0.25, 0.25, 0, 1, 0, 0.75, 0, 1, 0 },
		{ .50, .95, 0.45, 0.45, 0, 1, 0, 0.75, 1, 0, 0 },
		{ .50, .45, 0.75, 0.75, 0, 1, 0, 0.55, 0, 1, 0 },
		{ .50, .75, 0.65, 0.65, 0, 1, 0, 0.55, 1, 0, 0 },
		{ .50, .45, 0.45, 0.45, 0, 1, 0, 0.15, 0, 1, 0 },
		{ .90, .95, 0.25, 0.25, 1, 0, 0, 0.05, 0, 0, 1 },
		{ .95, .45, 0.15, 0.15, 0, 1, 0, 0.02, 0, 0, 1 }
	};
*/

	public void InititalizeANN ()
	{
		//first: input, second: output, third: hidden layer
		ANN.Initialize (INPUTNEURONS, OUTPUTNEURONS, HIDDENNEURONS);
		ANN.SetLearningRate (0.3);
		ANN.SetMomentum (true, 0.8);
		InputForNeurons = new double[TOTALNEURONS];
		for (int i = 0; i < TOTALNEURONS; i++)
			InputForNeurons [i] = 0;
	}

	public void TrainANN ()
	{
		double error = 1;
		int counter = 0;
		string message = "Training \n";
		message += ANN.ToStringData ();
		//	Debug.Log (message);
		while ((error > 0.01) && counter < 100000) {
			error = 0;
		
			for (int i = 0; i < TESTNUMBER; i++) {
				ANN.GiveInput (0, Trainingsset [i, 0]);
				ANN.GiveInput (1, Trainingsset [i, 1]);
				ANN.GiveInput (2, Trainingsset [i, 2]);
				ANN.GiveInput (3, Trainingsset [i, 3]);
				ANN.GiveInput (4, Trainingsset [i, 4]);
				ANN.GiveInput (5, Trainingsset [i, 5]);
				ANN.GiveInput (6, Trainingsset [i, 6]);
				ANN.GiveInput (7, Trainingsset [i, 7]);
		
				ANN.DesiredOutput (0, Trainingsset [i, 8]);
				ANN.DesiredOutput (1, Trainingsset [i, 9]);
				ANN.DesiredOutput (2, Trainingsset [i, 10]);

				ANN.FeedForward ();
				error += ANN.CalculateError ();
				ANN.BackPropagate ();

			}
			counter++;
			error = error / TESTNUMBER;
			//Debug.Log ("Training: error is" + error);
		}
		//Debug.Log (error);
		Debug.Log (message + "\n\n" + ANN.ToStringData ());
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
		ActivateWindows (false);
		closed = true;
		IsFighting = IsDead = IsEnemyFleeing = false;

		//Initialize the players, the network and train ANN
		InitializePlayer ();
		InititalizeANN ();
		TrainANN ();
	}

	private void ActivateWindows (bool value)
	{
		messageWindow.SetActive (value);
		battleWindow.SetActive (value);
		actionsWindow.SetActive (value);
		hudWindow.SetActive (value);
		Log.SetActive (value);
	}

	void OnDisable ()
	{
		ActivateWindows (false);
	}

	void OnEnable ()
	{
		//inventory = new Dictionary<String, Item> ();
		closeDelay = 2.0f;
		messageWindow.SetActive (false);
		battleWindow.SetActive (false);
		actionsWindow.SetActive (false);
		hudWindow.SetActive (true);
		Log.SetActive (true);
		closed = true;
		IsFighting = IsDead = false;

		//Attack = UnityEngine.Random.Range (10, 12);
		//Defense = UnityEngine.Random.Range (10, 12);
		//Gold = 0;
		//CurrentHealth = Health;
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

	public void InitializePlayer ()
	{
		Attack = UnityEngine.Random.Range (10, 12);
		Defense = UnityEngine.Random.Range (10, 12);
		Element = Random.Range (0, 3);
		Health = CurrentHealth = Random.Range (100, 200);
		Gold = Random.Range (100, 1000);
		inventory.Clear ();
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
		// Taking the players health for calculating the networks error
		playerStartHealth = CurrentHealth;
		enemyStartHealth = enemy.CurrentHealth;
		playerStartGold = Gold;
		playerSlider = GameObject.FindGameObjectWithTag ("PlayerHealth").gameObject.GetComponent<Slider> ();
		enemySlider = GameObject.FindGameObjectWithTag ("EnemyHealth").gameObject.GetComponent<Slider> ();
		UpdateGUI ();
		element = enemyElement = false;
		ElementDifferentiation ();
		//Feed the ANN with inputs and feed forward for output
		//FeedANN ();

		Invoke ("ShowActions", 1);
		IsEnemyFleeing = false;
		IsFighting = true;

	}

	public void CloseBattle ()
	{
		HideActions ();
		hudWindow.SetActive (true);
		battleWindow.SetActive (false);
		enemy = null;
		IsFighting = IsEnemyFleeing = false;
		//TRAIN ANN here -> IsDead -> good job boy!

		if (IsDead) {
			hudWindow.SetActive (false);
			IsFighting = true;
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
			IsFighting = true;
			IsDead = true;
			ShowMessage ("You died...", 1);
			Invoke ("CloseBattle", 2);
			RetrainANN (); //No changes to the Network- ANN beat player
		}


	}

	public void GameOver ()
	{
		InitializePlayer ();
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


		int enemyAttack = (int)UnityEngine.Random.Range (0, 4);
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
		case 3: 
			message += "Special Attack";
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
		int defense = (int)Math.Round ((double)(defenseMod + Defense) * DefenseFactor);
		Mathf.Max (CurrentHealth -= (damage - defense), 0);
		message += string.Format ("\nDamage received: {0}", (damage - defense));

		if (!IsDead && CurrentHealth > 0) {
			message += "\n You got away!";
			//ANN good enough that player decided to run
			RetrainANN ();
		}
			
		UpdateGUI ();
		ShowMessage (message, 1);
		ToLog (message);
		Invoke ("CloseBattle", 1);
		
	}

	// Deals damages to both characters, decides on the boost and weakness
	// 0= fast, 1 = norm, 2 = power; norm > fast > power > norm
	public void DealDamages (int id)
	{
		int enemyAttack = (int)UnityEngine.Random.Range (0, 4);
		bool boost = false;
		bool weaken = false;
		string message = "";
		//Feed network and receive the calculated outputs
		FeedANN ();
		if (CONTROL) {
			int enemyDecision = ANN.GetMaxOutput ();
			message += "Att: " + ANN.GetOutput (0).ToString ("F3") + "%;" + " Special: ";
			message += ANN.GetOutput (1).ToString ("F3") + "%;" + " Flee: " + ANN.GetOutput (2).ToString ("F3") + "%.\n";
			if (enemyDecision == 0) {
				enemyAttack = Random.Range (0, 3);
			} else if (enemyDecision == 1) {
				enemyAttack = 3;
			} else if (enemyDecision == 2) {
				EnemyFlees ();
				message += "Your enemy is trying to flee.";
				boost = true;
			} else
				Debug.Log ("ERROR FROM ANN ON GETMAXOUTPUT");
		}

		message += "You have chosen: ";
		//STRENGTHS & WEAKNESSES
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
		//Special attack - always boosts on correct element strength
		case 3:
			message += "Special Attack: " + GetElement (Element);
			boost = element;
			break;
		default:
			break;
		}
		if (!CONTROL) {
			if (Random.Range (0, 15) == 1) {
				EnemyFlees ();
				message += "\n Your enemy is trying to flee.";
				boost = true;
			}
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
			//on Special attack: always boost for enemy if correct element
			case 3:
				message += "Special Attack " + GetElement (enemy.Element);
				weaken = enemyElement;
				break;
			default:
				break;
			}
		}
	
		//CALCULATIONS
		int enemyDamage = enemy.Attack;
		int enemyDefense = enemy.Defense;
		//Enemy Gets Damage
		int damage = Attack + attackMod;
		if (boost)
			damage = Mathf.RoundToInt (damage * 1.25f);
		if (element)
			damage += Random.Range (6, 12);
		int defense = Mathf.RoundToInt (enemyDefense * DefenseFactor);
		if (element)
			defense -= Random.Range (3, 8);
		Mathf.Max (enemy.CurrentHealth -= (damage - defense), 0);
		message += string.Format ("\nDamage dealt: {0}", (damage - defense));

		if (!IsEnemyFleeing) {
			//Player Gets Damage
			damage = enemyDamage;
			if (weaken)
				damage = Mathf.RoundToInt (damage * 1.25f);
			if (enemyElement)
				damage += Random.Range (6, 12);
			defense = (int)Math.Round ((double)(defenseMod + Defense) * DefenseFactor);
			if (enemyElement)
				defense -= Random.Range (3, 8);
			Mathf.Max (CurrentHealth -= (damage - defense), 0);
			message += string.Format ("\nDamage received: {0}", (damage - defense));

		}

		//RESULTS
		if (!IsDead && enemy.CurrentHealth <= 0) {
			
			message += string.Format ("\nYou won! You earned {0} gold!", enemy.Gold);
			ToLog (message);
			ShowMessage (message, 2.5f);
			Gold += enemy.Gold;
			//CurrentHealth = Health;
			UpdateGUI ();
			//when the enemy dies -> correct decisions or wrong ones?
			ChangeDesiredOutput ();
			Invoke ("CloseBattle", 1);
		
		} else if (IsEnemyFleeing) {
			message += "\n The enemy got away!";
			//The enemy got away -> strengthen ANN
			ChangeDesiredOutput ();

			// Restock some health to the player
			CurrentHealth += (int)(CurrentHealth / 4);
			UpdateGUI ();
			ToLog (message);
			ShowMessage (message, 2);
			Invoke ("CloseBattle", 1);	

		} else {
			UpdateGUI ();
			ToLog (message);
			ShowMessage (message, 2);

		}
	}

	public void EnemyFlees ()
	{
		IsEnemyFleeing = true;
	}

		
	private void UpdateGUI ()
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
		logText.text += message + "\n";
	}

	public void ResetPlayerHealth ()
	{
		CurrentHealth = Health;
	}

	//Decides on the strength and weakness of the player vs enemy
	public void ElementDifferentiation ()
	{
		element = false;
		enemyElement = false;
		if (Element == 0) {
			if (enemy.Element == 2)
				element = true;
		} else if (Element == 1) {
			if (enemy.Element == 0)
				element = true;
	
		} else if (Element == 2) {
			if (enemy.Element == 1)
				element = true;
		}

		switch (enemy.Element) {
		case 0:
			if (Element == 2)
				enemyElement = true;
			break;
		case 1:
			if (Element == 0)
				enemyElement = true;
			break;
		case 2: 
			if (Element == 1)
				enemyElement = true;
			break;
		default:
			break;
		}
	}


	//Health: min = 0, max = 200
	//Gold: min = 0, max = 5000
	//Stats: min = -100, max = 100
	public double Standardization (int x, double min, double max)
	{
		return ((double)((x - min) / (max - min)));
	}


	//Feed the ANN with input
	//Health, Player Health, Difference Att, Difference Def, Class difference (3 input), Gold of player ||out: att, special, flee
	//Class difference : 001 good, 010 norm, 100 bad
	public void FeedANN ()
	{
		InputForNeurons [0] = Standardization (enemy.CurrentHealth, HEALTHMIN, HEALTHMAX);
		InputForNeurons [1] = Standardization (CurrentHealth, HEALTHMIN, HEALTHMAX);
		InputForNeurons [2] = Standardization (enemy.Attack - (Attack + attackMod), DIFFMIN, DIFFMAX);
		InputForNeurons [3] = Standardization (enemy.Defense - (Defense + defenseMod), DIFFMIN, DIFFMAX);
		if (element) {
			InputForNeurons [4] = 1;
			InputForNeurons [5] = 0;
			InputForNeurons [6] = 0;

		} else if (enemyElement) {
			InputForNeurons [4] = 0;
			InputForNeurons [5] = 0;
			InputForNeurons [6] = 1;
		} else {
			InputForNeurons [4] = 0;
			InputForNeurons [5] = 1;
			InputForNeurons [6] = 0;
		}
		InputForNeurons [7] = Standardization (Gold, GOLDMIN, GOLDMAX);

		
		for (int i = 0; i < INPUTNEURONS; i++) {
			ANN.GiveInput (i, InputForNeurons [i]);
		}
		ANN.FeedForward (); //calculate the outputs
		// pass them back to the array
		InputForNeurons [8] = ANN.GetOutput (0);
		InputForNeurons [9] = ANN.GetOutput (1);
		InputForNeurons [10] = ANN.GetOutput (2);
		//Debug.Log (ANN.ToStringData ());
	}

	// strengthen or weaken the weights
	private void RetrainANN ()
	{
		double error = 1;
		int counter = 0;
		//Set the 


		//Set Neurons
		for (int i = 0; i < INPUTNEURONS; i++) {
			ANN.GiveInput (i, InputForNeurons [i]);
		}
		ANN.DesiredOutput (0, InputForNeurons [8]);
		ANN.DesiredOutput (1, InputForNeurons [9]);
		ANN.DesiredOutput (2, InputForNeurons [10]);

		while ((error > 0.05) && (counter < 40000)) {
			//only 1 training set -> no measure error calc required
			ANN.FeedForward ();
			error = ANN.CalculateError ();
			ANN.BackPropagate ();
			counter++;
		}
		Debug.Log ("Retraining network!\n" + ANN.ToStringData ());
	}

	// Did the enemy decide correctly? if yes -> strengthen ANN, else weaken
	private void ChangeDesiredOutput ()
	{
		//depending on the pattern we decide to Retrain
		switch (ANN.GetMaxOutput ()) {
		case 0:
			//Attack - enemy died , player survived- enough damage?
			if ((CurrentHealth > (int)(playerStartHealth * 0.8f))) {
				//if not even 20% damage were done ->flee!
				InputForNeurons [0] = Standardization (enemyStartHealth, HEALTHMIN, HEALTHMAX);
				InputForNeurons [1] = Standardization (playerStartGold, HEALTHMIN, HEALTHMAX);
				InputForNeurons [7] = Standardization (playerStartGold, GOLDMIN, GOLDMAX);
				InputForNeurons [8] = 0;
				InputForNeurons [9] = 0;
				InputForNeurons [10] = 1;
			}
			RetrainANN ();
			break;
		case 1: 
			//Special
			if ((CurrentHealth > (int)(playerStartHealth * 0.8f))) {
				//enemy died and didnt damage enough - try fleeing next time
				InputForNeurons [0] = Standardization (enemyStartHealth, HEALTHMIN, HEALTHMAX);
				InputForNeurons [1] = Standardization (playerStartGold, HEALTHMIN, HEALTHMAX);
				InputForNeurons [7] = Standardization (playerStartGold, GOLDMIN, GOLDMAX);
				InputForNeurons [8] = 0;
				InputForNeurons [9] = 0;
				InputForNeurons [10] = 1;
			}
			RetrainANN ();
			break;
		case 2: 
			//Flee - if alive > strengthen ANN else wrong decision
			// else -> no use of running- try damaging the user
			if (!(enemy.CurrentHealth >= 0)) {
				int random = Random.Range (0, 2);
				if (random == 0) {
					//if the enemy dies - try to go with att
					InputForNeurons [8] = 1;
					InputForNeurons [9] = 0;
					InputForNeurons [10] = 0;
				} else {
					//if the enemy dies - try to go with special
					InputForNeurons [8] = 0;
					InputForNeurons [9] = 1;
					InputForNeurons [10] = 0;
				}
			}
			RetrainANN ();
			break;
		default:
			break;
		}
	}


}
