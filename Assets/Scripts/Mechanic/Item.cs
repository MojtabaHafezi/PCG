//Minor changes were made to the original class
/***************************************************************************************
*    Title: Procedural Content Generation for Unity Game Development
* 	 Chapter 5
*    Author: Ryan Watkins
*    Date: 2016
*    Code version: unknown
*    Availability: https://www.packtpub.com/game-development/procedural-content-generation-unity-game-development
***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;


public enum itemType
{
	glove,
	boot,
	weapon,
	body,
	helm,
	shield
}


public class Item : MonoBehaviour
{
	public Sprite glove;
	public Sprite boot;

	public itemType type;
	public Color level;
	public int attackMod, defenseMod;

	private SpriteRenderer spriteRenderer;

	public Item (Item item)
	{
		this.type = item.type;
		this.attackMod = item.attackMod;
		this.defenseMod = item.defenseMod;
		this.spriteRenderer = item.spriteRenderer;
	}

	public Item ()
	{
		
	}

	public virtual void RandomItemInit ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		SelectItem ();
	}

	private void SelectItem ()
	{
		var itemCount = Enum.GetValues (typeof(itemType)).Length;
		type = (itemType)Random.Range (0, 2); //itemcount when all items are implemented

		switch (type) {
		case itemType.glove:
			attackMod = Random.Range (1, 4);
			defenseMod = 0;
			spriteRenderer.sprite = glove;
			break;
		case itemType.boot:
			attackMod = 0;
			defenseMod = Random.Range (1, 4);
			spriteRenderer.sprite = boot;
			break;
		}

		int randomLevel = Random.Range (0, 100);
		if (randomLevel >= 0 && randomLevel < 60) {
			spriteRenderer.color = level = Color.gray;
			attackMod += Random.Range (1, 4);
			defenseMod += Random.Range (1, 4);
		} else if (randomLevel >= 60 && randomLevel < 85) {
			spriteRenderer.color = level = Color.green;
			attackMod += Random.Range (4, 10);
			defenseMod += Random.Range (4, 10);
		} else if (randomLevel >= 85 && randomLevel < 98) {
			spriteRenderer.color = level = Color.yellow;
			attackMod += Random.Range (15, 25);
			defenseMod += Random.Range (15, 25);
		} else {
			spriteRenderer.color = level = Color.magenta;
			attackMod += Random.Range (40, 55);
			defenseMod += Random.Range (40, 55);
		}
	}
}
