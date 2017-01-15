using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//Singleton pattern
	public static GameManager instance = null;

	public BoardManager boardScript;
	private  int lvl = 3;


	void Awake ()
	{
		if (instance == null) {
			instance = this; 
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent<BoardManager> ();
	
	}


	public void InitGame ()
	{
		boardScript.SetupScene (lvl);
	}
	


}
