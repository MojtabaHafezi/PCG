﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUIAttributes : MonoBehaviour
{

	public Text playerHP;
	public Text playerGP;
	public Text playerAtt;
	public Text playerDef;
	public Text playerSp;

	public Text enemyHP;
	public Text enemyGP;
	public Text enemyAtt;
	public Text enemyDef;
	public Text enemySp;

	public void UpdateGUI ()
	{
		playerHP.text = "HP: " + PlayerManager.instance.CurrentHealth;
		playerGP.text = "GP: " + PlayerManager.instance.Gold;
		playerAtt.text = "Att: " + (PlayerManager.instance.Attack + PlayerManager.instance.attackMod);
		playerDef.text = "Def: " + (PlayerManager.instance.Defense + PlayerManager.instance.defenseMod);
		playerSp.text = "SP: " + PlayerManager.instance.GetElement (PlayerManager.instance.Element);

		enemyHP.text = "HP: " + PlayerManager.instance.enemy.CurrentHealth;
		enemyGP.text = "GP: " + PlayerManager.instance.enemy.Gold;
		enemyAtt.text = "Att: " + PlayerManager.instance.enemy.Attack;
		enemyDef.text = "Def: " + PlayerManager.instance.enemy.Defense;
		enemySp.text = "SP: " + PlayerManager.instance.GetElement (PlayerManager.instance.enemy.Element);
	}
}
