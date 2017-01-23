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
		GameObject playerInstance = Instantiate (player, new Vector3 (2, 2, 0), Quaternion.identity);
		playerInstance.transform.SetParent (boardHolder);
	}

}
