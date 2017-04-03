
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
using UnityEngine.UI;
using System;

public class Player : Movement
{
	public float restartLevelDelay = 2f;
	//Delay time in seconds to restart level.


	public AudioClip item1;
	public AudioClip item2;
	public AudioClip item3;
	public AudioClip item4;

	public Animator animator;

	//Start overrides the Start function of MovingObject
	protected override void Start ()
	{

		//Call the Start function of the MovingObject base class.
		base.Start ();
		Camera.main.transform.parent = this.transform;
		Camera.main.transform.position = this.transform.position;
		//CameraController.instance.InDungeon ();


	}

	private void Update ()
	{
		//Only move out of battle
		if (!PlayerManager.instance.isFighting) {
			
		
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

		//PCGGameManager.instance.UpdateBoard (horizontal, vertical);

	}

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
		if (other.tag == "Exit") {

			//TODO: Animation?
			//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
			//Invoke ("Restart", restartLevelDelay);
			Restart ();
		}

		//Check if the tag of the trigger collided with is PCGExit.
		if (other.tag == "PCGExit") {

			//TODO: Animation?
			//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
			//Invoke ("Restart", restartLevelDelay);
			RestartPCG ();
		}

		//Check if the tag of the trigger collided with is Food.
		else if (other.tag == "Chest") {

			//Add a random sound and item, only once though
			if (!other.gameObject.GetComponent<Chest> ().IsOpen) {
				//TODO: SHOW INTERFACE WITH GENERATED ITEM
				AudioManager.instance.RandomizeSfx (item1, item2, item3, item4);

				other.gameObject.GetComponent<Chest> ().Open ();

				if (other.gameObject.GetComponent<Chest> ().randomItem != null) {
					PlayerManager.instance.UpdateInventory (other.gameObject.GetComponent<Chest> ().randomItem);
				} else {
					if (other.gameObject.GetComponent<Chest> ().weapon != null) {
						PlayerManager.instance.UpdateInventory (other.gameObject.GetComponent<Chest> ().weapon);
					}
				}
			}
			//Disable the  object the player collided with.
			//other.gameObject.SetActive (false);
		}

		//Check if the tag of the trigger collided with is Enemy.
		else if (other.tag == "Enemy") {

			//Debug.Log ("Ready for a battle");

			//TODO: ANIMATION OR ANYTHING FOR COMMENCING BATTLE
			//TODO: START A BATTLE HERE
			PlayerManager.instance.StartBattle (other.gameObject);

			//Disable the object the player collided with if the battle is over (even on running away)
			other.gameObject.SetActive (false);
		}
	}


	//Restart reloads the scene when called.
	private void Restart ()
	{
		CameraController.instance.OutDungeon ();
		Destroy (GameObject.Find ("Board"));
		GameObject.FindObjectOfType<GameManager> ().InitGame ();
	}


	private void RestartPCG ()
	{
		CameraController.instance.OutDungeon ();
		Destroy (GameObject.Find ("Board"));
		GameObject.FindObjectOfType<PCGGameManager> ().InitGame ();
	}
}