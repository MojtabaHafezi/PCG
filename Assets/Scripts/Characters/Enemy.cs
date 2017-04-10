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
		int random = Random.Range (0, 6);
		float factor = 0f;
		switch (random) {
		case 0:
			factor = 0.15f; 
			break;
		case 1:
			factor = 0.25f;
			break;
		case 2:
			factor = 0.35f;
			break;
		case 3:
			factor = 0.75f;
			break;
		case 4: 
			factor = 0.95f;
			break;
		case 5: 
			factor = 1.25f;
			break;
		default:
			break;
		}
		Attack = Random.Range (6, 14) + ((int)(PlayerManager.instance.attackMod * factor));
		Defense = Random.Range (6, 14) + ((int)(PlayerManager.instance.defenseMod * factor));

		Element = Random.Range (0, 3);
		Gold = Random.Range (0, 100);
		Health = CurrentHealth = Random.Range (60, 200);
		
	}


}
