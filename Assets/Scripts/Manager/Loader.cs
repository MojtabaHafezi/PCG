using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checks if the gameManager was instantiated, if not does it
public class Loader : MonoBehaviour
{
	public GameObject gameManager;

	// Use this for initialization
	void Awake ()
	{
		if (GameManager.instance == null) {
			Instantiate (gameManager);
		}
	}
}
