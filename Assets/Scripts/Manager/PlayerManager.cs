using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance = null;
	//Player stats related components
	public int Health{ get; set; }

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
	public bool isFighting;




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
		closed = true;
		isFighting = false;
	}

	public  void UpdateInventory (Item item)
	{

		Item itemData = item;
		ShowMessage (string.Format ("Found an item with attack {0} and def {1}", item.attackMod, item.defenseMod), 2.0f);


		switch (itemData.type) {
		case itemType.glove:
			
			inventory ["glove"] = itemData;
			
			
			break;
		case itemType.boot:

			inventory ["boot"] = itemData;


			break;
		case itemType.weapon:
			
			inventory ["weapon"] = itemData;
				
		
			break;
		}

		attackMod = 0;
		defenseMod = 0;

		foreach (KeyValuePair<String, Item> gear in inventory) {
			attackMod += gear.Value.attackMod;
			defenseMod += gear.Value.defenseMod;
		}

	}

	public void ShowMessage (string message, float delay)
	{
		messageWindow.SetActive (false);
		messageWindow.SetActive (true);
		messageWindow.GetComponentInChildren<Text> ().text = message;
		closeDelay = delay;
		delay = 0;
		closed = false;
	}

	public void ShowActions ()
	{
		actionsWindow.SetActive (true);
	}

	public void HideActions ()
	{
		actionsWindow.SetActive (false);
	}

	public void StartBattle (GameObject enemy)
	{
		battleWindow.SetActive (true);
		ShowMessage ("Wild monster encountered!", 1.0f);
		Invoke ("ShowActions", 1);
		
		isFighting = true;
	}

	public void CloseBattle ()
	{
		battleWindow.SetActive (false);
		HideActions ();
		isFighting = false;

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
	}


	public void Action (int id)
	{
		HideActions ();
		ShowMessage ("You chose your attack!", 1);
		Invoke ("ShowActions", 1);

	}

	public void Flee ()
	{

		CloseBattle ();
		
	}



}
