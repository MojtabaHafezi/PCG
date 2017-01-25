//Minor changes were made to the original class
/***************************************************************************************
*    Title: Procedural Content Generation for Unity Game Development
* 	 Chapter 3-7
*    Author: Ryan Watkins
*    Date: 2016
*    Code version: unknown
*    Availability: https://www.packtpub.com/game-development/procedural-content-generation-unity-game-development
***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGGameManager : MonoBehaviour
{

	public static PCGGameManager instance = null;
	//Static instance of GameManager which allows it to be accessed by any other script.

	//CHAPTER 3
	private PCGBoardManager boardScript;
	//Store a reference to our BoardManager which will set up the level.

	//Awake is always called before any Start functions
	void Awake ()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);	

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);

		//CHAPTER 3
		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<PCGBoardManager> ();

	}

	//Initializes the game for each level.
	public void InitGame ()
	{
		//Call the SetupScene function of the BoardManager script, pass it current level number.
		//boardScript.BoardSetup ();
		boardScript.StartDungeon ();
	}

	//	public void UpdateBoard (int horizantal, int vertical)
	//	{
	//		boardScript.addToBoard (horizantal, vertical);
	//	}
}
