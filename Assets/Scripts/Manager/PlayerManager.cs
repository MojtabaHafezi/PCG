using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance = null;
	//Player stats related components
	public int health{ get; set; }

	//Equipment/Inventory related components
	public int attackMod, defenseMod;
	private Dictionary<String, Item> inventory;


	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

	}

	void Start ()
	{
		inventory = new Dictionary<String, Item> ();

	}

	public  void UpdateInventory (Item item)
	{
		Item itemData = item;

		switch (itemData.type) {
		case itemType.glove:
			if (!inventory.ContainsKey ("glove"))
				inventory.Add ("glove", itemData);
			else
				inventory ["glove"] = itemData;
			break;
		case itemType.boot:
			if (!inventory.ContainsKey ("boot"))
				inventory.Add ("boot", itemData);
			else
				inventory ["boot"] = itemData;
			break;
		case itemType.weapon:
			if (!inventory.ContainsKey ("weapon"))
				inventory.Add ("weapon", itemData);
			else
				inventory ["weapon"] = itemData;
			break;
		}

		attackMod = 0;
		defenseMod = 0;

		foreach (KeyValuePair<String, Item> gear in inventory) {
			attackMod += gear.Value.attackMod;
			defenseMod += gear.Value.defenseMod;
			Debug.Log (gear.ToString () + gear.Value.attackMod + gear.Value.defenseMod);
		}
	}



}
