using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy
{

	//Enemy stats related components
	public int Health{ get; set; }

	public int CurrentHealth{ get; set; }

	public int Attack { get; set; }

	public int Defense{ get; set; }

	public int Gold { get; set; }

	public int Element{ get; set; }
	// Use this for initialization

	public Enemy ()
	{
		Instantiate ();
	}

	public void Instantiate ()
	{
		Attack = UnityEngine.Random.Range (6, 12) + Mathf.Abs (PlayerManager.instance.attackMod / 2);
		Defense = UnityEngine.Random.Range (6, 12) + Mathf.Abs (PlayerManager.instance.defenseMod / 2);
		Element = Random.Range (0, 3);
		Gold = Random.Range (0, 100);
		Health = CurrentHealth = Random.Range (40, 125);
		
	}


}
