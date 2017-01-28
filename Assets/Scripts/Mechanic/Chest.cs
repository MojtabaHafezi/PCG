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

public class Chest : MonoBehaviour
{

	public Sprite openSprite;
	public Item randomItem;

	public Weapon weapon;

	private SpriteRenderer spriteRenderer;

	void Awake ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void Open ()
	{
		spriteRenderer.sprite = openSprite;

		GameObject toInstantiate = new GameObject ();

		if (Random.Range (0, 2) == 1) {
			randomItem.RandomItemInit ();
			toInstantiate = randomItem.gameObject;
		} else {
			weapon.AquireWeapon ();
			toInstantiate = weapon.gameObject;
		}
		GameObject instance = Instantiate (toInstantiate, new Vector3 (transform.position.x, transform.position.y, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (GameObject.Find ("Board").transform);
		gameObject.layer = 10;
		spriteRenderer.sortingLayerName = "Items";
	}
}
