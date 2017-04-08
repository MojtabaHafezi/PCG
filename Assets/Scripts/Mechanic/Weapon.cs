using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
	private WeaponComponents[] weaponsComps;

	public Weapon (Weapon weapon)
	{
		this.type = weapon.type;
		this.attackMod = weapon.attackMod;
		this.defenseMod = weapon.defenseMod;
	}

	public void CreateWeapon ()
	{
		weaponsComps = GetComponentsInChildren<WeaponComponents> ();
		RandomWeapon ();
		// Changing the color of ea child weapon component
		EnableSpriteRender (true);
	}

	public override void RandomItemInit ()
	{
		//base.RandomItemInit ();
		CreateWeapon ();
	}

	public void RandomWeapon ()
	{
		attackMod = defenseMod = 0;

		type = itemType.weapon;
		int randomLevel = Random.Range (0, 100);
		if (randomLevel >= 0 && randomLevel < 60) {
			
			level = Color.gray;
			attackMod += Random.Range (1, 4);
			defenseMod += Random.Range (1, 4);
		} else if (randomLevel >= 60 && randomLevel < 85) {
			level = Color.green;
			attackMod += Random.Range (4, 10);
			defenseMod += Random.Range (4, 10);
		} else if (randomLevel >= 85 && randomLevel < 98) {
			level = Color.yellow;
			attackMod += Random.Range (15, 25);
			defenseMod += Random.Range (15, 25);
		} else {
			level = Color.magenta;
			attackMod += Random.Range (40, 55);
			defenseMod += Random.Range (40, 55);
		}
		//Debug.Log (string.Format ("Weapon created with att: {0}, def:{1}, type: {2})", attackMod, defenseMod, type));

	}

	public void EnableSpriteRender (bool isEnabled)
	{
		foreach (WeaponComponents comp in weaponsComps) {
			comp.EnableSpriteRenderer ();
			comp.getSpriteRenderer ().enabled = true;
			comp.getSpriteRenderer ().color = level;
		}
	}

	public Sprite GetComponentImage (int index)
	{
		return weaponsComps [index].getSpriteRenderer ().sprite;
	}
}
