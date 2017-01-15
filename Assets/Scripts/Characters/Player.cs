
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
			//Call AttemptMove passing in the generic parameter, Pass in horizontal and vertical as parameters to specify the direction to move Player in.
			AttemptMove<Wall> (horizontal, vertical);
		}


	}

	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove <T> (xDir, yDir);

		//Hit allows us to reference the result of the Linecast done in Move.
		RaycastHit2D hit;

		//If Move returns true, meaning Player was able to move into an empty space.
		if (Move (xDir, yDir, out hit)) {
			//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
		}
	}


	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove <T> (T component)
	{
		Debug.Log ("This should not be called");
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
		else if (other.tag == "Item") {

			//TODO: SHOW INTERFACE WITH GENERATED ITEM
			Debug.Log ("Item found");
			//Disable the food object the player collided with.
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
		
		GameObject.FindObjectOfType<GameManager> ().InitGame ();

	}


	//CheckIfGameOver checks if the player is out of health points and if so, ends the game.
	private void CheckIfGameOver ()
	{

	}
}