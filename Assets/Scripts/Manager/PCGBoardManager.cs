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
using System;
using Random = UnityEngine.Random;

public class PCGBoardManager : MonoBehaviour
{
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

	public int columns = 5;
	public int rows = 5;

	public GameObject exit;

	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] outerWallTiles;
	public GameObject chestTile;

	//public GameObject enemy;
	public GameObject player;
	public GameObject playerInstance;

	private Transform boardHolder;
	private Dictionary<Vector2, Vector2> gridPositions = new Dictionary<Vector2, Vector2> ();

	private Transform dungeonBoardHolder;

	public void BoardSetup ()
	{
		boardHolder = new GameObject ("Board").transform;
		boardHolder.transform.SetParent (this.gameObject.transform);


		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				if (!gridPositions.ContainsKey (new Vector2 (x, y))) {
					gridPositions.Add (new Vector2 (x, y), new Vector2 (x, y));
				}


				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

				GameObject instance =
					Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (boardHolder);
			}
		}


		//Add the player instance into the board
		playerInstance = Instantiate (player, new Vector3 (2, 2, 0), Quaternion.identity) as GameObject;
		playerInstance.transform.SetParent (boardHolder);
	}

	private void addTiles (Vector2 tileToAdd)
	{
		if (!gridPositions.ContainsKey (tileToAdd)) {
			gridPositions.Add (tileToAdd, tileToAdd);
			GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
			GameObject instance = Instantiate (toInstantiate, new Vector3 (tileToAdd.x, tileToAdd.y, 0f), Quaternion.identity) as GameObject;
			instance.transform.SetParent (boardHolder);

			//Choose at random a wall tile to lay
			if (Random.Range (0, 3) == 1) {
				toInstantiate = wallTiles [Random.Range (0, wallTiles.Length)];
				instance = Instantiate (toInstantiate, new Vector3 (tileToAdd.x, tileToAdd.y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	public void addToBoard (int horizontal, int vertical)
	{
		if (horizontal == 1) {
			//Check if tiles exist
			int x = (int)playerInstance.transform.position.x;
			int sightX = x + 2;
			for (x += 1; x <= sightX; x++) {
				int y = (int)playerInstance.transform.position.y;
				int sightY = y + 1;
				for (y -= 1; y <= sightY; y++) {
					addTiles (new Vector2 (x, y));
				}
			}
		} else if (horizontal == -1) {
			int x = (int)playerInstance.transform.position.x;
			int sightX = x - 2;
			for (x -= 1; x >= sightX; x--) {
				int y = (int)playerInstance.transform.position.y;
				int sightY = y + 1;
				for (y -= 1; y <= sightY; y++) {
					addTiles (new Vector2 (x, y));
				}
			}
		} else if (vertical == 1) {
			int y = (int)playerInstance.transform.position.y;
			int sightY = y + 2;
			for (y += 1; y <= sightY; y++) {
				int x = (int)playerInstance.transform.position.x;
				int sightX = x + 1;
				for (x -= 1; x <= sightX; x++) {
					addTiles (new Vector2 (x, y));
				}
			}
		} else if (vertical == -1) {
			int y = (int)playerInstance.transform.position.y;
			int sightY = y - 2;
			for (y -= 1; y >= sightY; y--) {
				int x = (int)playerInstance.transform.position.x;
				int sightX = x + 1;
				for (x -= 1; x <= sightX; x++) {
					addTiles (new Vector2 (x, y));
				}
			}
		}
	}

}
