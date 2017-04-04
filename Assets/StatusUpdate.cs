using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attributes : MonoBehaviour
{
	public Text HpPlayer;
	public Text GpPlayer;
	public Text AttPlayer;
	public Text DefPlayer;

	public Text HpEnemy;
	public Text GpEnemy;
	public Text AttEnemy;
	public Text DefEnemy;


	public void UpdateText ()
	{
		HpPlayer.text = "HP: " + PlayerManager.instance.CurrentHealth;
		GpPlayer.text = "GP: " + PlayerManager.instance.Gold;
		AttPlayer.text = "Att: " + (PlayerManager.instance.Attack + PlayerManager.instance.attackMod);
		DefPlayer.text = "Def: " + (PlayerManager.instance.Defense + PlayerManager.instance.defenseMod);

		HpEnemy.text = "HP: " + PlayerManager.instance.enemy.CurrentHealth;
		GpEnemy.text = "HP: " + PlayerManager.instance.enemy.Gold;
		AttEnemy.text = "Att: " + PlayerManager.instance.enemy.Attack;
		DefEnemy.text = "Def: " + PlayerManager.instance.enemy.Defense;
	}
}
