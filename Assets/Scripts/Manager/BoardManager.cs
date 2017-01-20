//Minor changes were made to the original BoardManager class
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
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
	//for modifying how Count appears on editor/inspector
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}


	// game boards size
	public int columns = 16;
	public int rows = 16;
	//number of possible enemies/items to be encountered
	public Count itemCount = new Count (2, 9);
	public Count enemyCount = new Count (10, 18);

	public GameObject exit;
	public GameObject warp;

	//arrays are filled with prefabs in the editor.
	public GameObject[] floorTiles;
	public GameObject[] enemyTiles;
	public GameObject[] itemTiles;
	public GameObject outerWallTile;
	public GameObject player;

	//to keep the hierarchy clean (all generated objects get in here)
	private Transform boardHolder;
	//Track all the different possible positions on the grid and if already covered or free
	private List<Vector3> gridPositions = new List<Vector3> ();


	private void InitialiseList ()
	{
		//Clear the list first
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				//List of possible positions for all objects to be generated on
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	//to set up the outer wall and the floor
	private void BoardSetup ()
	{
		boardHolder = new GameObject ("Board").transform;
		boardHolder.transform.SetParent (this.gameObject.transform);
		//choosing the tile to be instantiated randomly
		int randomTile = Random.Range (0, floorTiles.Length);

		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTiles [randomTile];
				//except if its outerwall
				if (x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = outerWallTile;
				//instantiate the gameobject
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity);
				//putting the instantiated object as child of the boardholder
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	private Vector3 RandomPosition ()
	{
		//a random number between 0 and the number of available positions stored in the list
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		//to make sure 2 objects don't generate on the same spot -> remove 
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	private void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
	{
		//control how many of a given object to spawn
		int objectCount = Random.Range (minimum, maximum);

		//instantiate the
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			GameObject instance = Instantiate (tileChoice, randomPosition, Quaternion.identity);
			instance.transform.SetParent (boardHolder);
		}
	}

	public void SetupScene (int level)
	{
		//Create outer walls and floors
		BoardSetup ();
		//Reset available position
		InitialiseList ();
		//Instantiate random number of items and enemies
		LayoutObjectAtRandom (itemTiles, itemCount.minimum, itemCount.maximum);
		LayoutObjectAtRandom (enemyTiles, enemyCount.minimum, enemyCount.maximum);
		//Adding the warp as exit to the boarder object --- TODO: Random position
		//	GameObject instance = Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		GameObject instance = Instantiate (exit, RandomPosition (), Quaternion.identity);

		instance.transform.SetParent (boardHolder);

		//Add the player instance into the board
		instance = Instantiate (player, RandomPosition (), Quaternion.identity);
		instance.transform.SetParent (boardHolder);
		//player.transform.position = RandomPosition ();


	}
}
