using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUIAttributes : MonoBehaviour
{

	public Text PlayerHP;
	public Text PlayerGP;
	public Text PlayerAtt;
	public Text PlayerDef;

	public Text EnemyHP;
	public Text EnemyGP;
	public Text EnemyAtt;
	public Text EnemyDef;

	public void UpdateGUI ()
	{
		PlayerHP.text = "HP: " + PlayerManager.instance.CurrentHealth;
		PlayerGP.text = "GP: " + PlayerManager.instance.Gold;
		PlayerAtt.text = "Att: " + (PlayerManager.instance.Attack + PlayerManager.instance.attackMod);
		PlayerDef.text = "Def: " + (PlayerManager.instance.Defense + PlayerManager.instance.defenseMod);

		EnemyHP.text = "HP: " + PlayerManager.instance.enemy.CurrentHealth;
		EnemyGP.text = "GP: " + PlayerManager.instance.enemy.Gold;
		EnemyAtt.text = "Att: " + PlayerManager.instance.enemy.Attack;
		EnemyDef.text = "Def: " + PlayerManager.instance.enemy.Defense;
	}
}
