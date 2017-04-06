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
		Gold = 0;
		Health = CurrentHealth = 100;
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
		Item itemData = item;
		String message = "Found ";
		Item getItem;
		switch (itemData.type) {
		case itemType.glove:
			message += "gloves";
			if (inventory.ContainsKey ("glove")) {
				inventory.TryGetValue ("glove", out getItem);
				if ((getItem.attackMod + getItem.defenseMod) < (itemData.attackMod + itemData.defenseMod))
					inventory ["glove"] = itemData;
			} else {
				inventory ["glove"] = itemData;
			}
			break;
		case itemType.boot:
			message += "boots";
			if (inventory.ContainsKey ("boot")) {
				inventory.TryGetValue ("boot", out getItem);
				if ((getItem.attackMod + getItem.defenseMod) < (itemData.attackMod + itemData.defenseMod))
					inventory ["boot"] = itemData;
			} else {
				inventory ["boot"] = itemData;
			}
			break;
		case itemType.weapon:
			message += "a weapon";
			if (inventory.ContainsKey ("weapon")) {
				inventory.TryGetValue ("weapon", out getItem);
				if ((getItem.attackMod + getItem.defenseMod) < (itemData.attackMod + itemData.defenseMod))
					inventory ["weapon"] = itemData;
			} else {
				inventory ["weapon"] = itemData;
			}
			break;
		}
		message += string.Format (" with an attack of {0} and a defense of {1}", item.attackMod, item.defenseMod);
		ShowMessage (message, 2.0f);

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

		Invoke ("CloseBattle", 1);
		
	}

	// Deals damages to both characters, decides on the boost and weakness
	// 0= fast, 1 = norm, 2 = power; norm > fast > power > norm
	public void DealDamages (int id)
	{
		string message = "You have chosen: ";
		int enemyAttack = (int)UnityEngine.Random.Range (0, 3);
		bool boost = false;
		bool weaken = false;

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
		default:
			break;
		}
		if (Random.Range (0, 15) == 1) {
			enemyAttack = 3;
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
			default:
				break;
			}
		}
	

		int enemyDamage = enemy.Attack;
		int enemyDefense = enemy.Defense;
		//Enemy Gets Damage
		int damage = Attack + attackMod;
		if (boost)
			damage = Mathf.RoundToInt (damage * 1.5f);
		int defense = Mathf.RoundToInt (enemyDefense * 0.25f);
		Mathf.Max (enemy.CurrentHealth -= (damage - defense), 0);
		message += string.Format ("\nDamage dealt: {0}", (damage - defense));

		if (!IsEnemyFleeing) {
			//Player Gets Damage
			damage = enemyDamage;
			if (weaken)
				damage = Mathf.RoundToInt (damage * 1.5f);
			defense = (int)Math.Round ((double)(defenseMod + Defense) * 0.25);
			Mathf.Max (CurrentHealth -= (damage - defense), 0);
			message += string.Format ("\nDamage received: {0}", (damage - defense));

		}

		if (!IsDead && enemy.CurrentHealth <= 0) {
			
			message += string.Format ("\nYou won! You earned {0} gold!", enemy.Gold);
			ShowMessage (message, 2.5f);
			Gold += enemy.Gold;
			CurrentHealth = Health;
			UpdateStats ();
			Invoke ("CloseBattle", 1);	
		} else if (IsEnemyFleeing) {
			message += "\n The enemy got away!";
			CurrentHealth = Health;
			UpdateStats ();
			ShowMessage (message, 2);
			Invoke ("CloseBattle", 1);	
		} else {
			UpdateStats ();
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



}
