﻿
//Major changes were made to the original player script.
/***************************************************************************************
*    Title: Scavenger
*    Author: Unity Technologies
*    Date: 2017
*    Code version: unknown
*    Availability: https://unity3d.com/de/learn/tutorials/projects/2d-roguelike-tutorial
***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movement
{
	public float restartLevelDelay = 1f;
	//Delay time in seconds to restart level.


	public AudioClip item1;
	public AudioClip item2;
	public AudioClip item3;
	public AudioClip item4;


	// TODO: STATS




	//Start overrides the Start function of MovingObject
	protected override void Start ()
	{

		//Call the Start function of the MovingObject base class.
		base.Start ();
	}

	private void Update ()
	{
		
		int horizontal = 0;     //Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.


		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int)(Input.GetAxisRaw ("Horizontal"));

		//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
		vertical = (int)(Input.GetAxisRaw ("Vertical"));

		//Check if moving horizontally, if so set vertical to zero.
		if (horizontal != 0) {
			
			vertical = 0;
		}

		//Check if we have a non-zero value for horizontal or vertical
		if (horizontal != 0 || vertical != 0) {
			
			Move (horizontal, vertical);
		
		}


	}

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
		if (other.tag == "Exit") {
			//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
			Invoke ("Restart", restartLevelDelay);
		}

		//Check if the tag of the trigger collided with is Food.
		else if (other.tag == "Items") {

			//TODO: SHOW INTERFACE WITH GENERATED ITEM
			Debug.Log ("Item found - generation needs to be done");
			//Disable the food object the player collided with.
			AudioManager.instance.RandomizeSfx (item1, item2, item3, item4);
			other.gameObject.SetActive (false);
		}

		//Check if the tag of the trigger collided with is Enemy.
		else if (other.tag == "Enemy") {

			Debug.Log ("Ready for a battle");

			//TODO: ANIMATION OR ANYTHING FOR COMMENCING BATTLE
			//TODO: START A BATTLE HERE

			//Disable the object the player collided with if the battle is over (even on running away)
			other.gameObject.SetActive (false);
		}
	}


	//Restart reloads the scene when called.
	private void Restart ()
	{

		Destroy (GameObject.Find ("Board"));
		GameObject.FindObjectOfType<GameManager> ().InitGame ();

	}


	//CheckIfGameOver checks if the player is out of health points and if so, ends the game.
	private void CheckIfGameOver ()
	{

	}
}