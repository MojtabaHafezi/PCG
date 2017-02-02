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

public enum TileType
{
	essential,
	random,
	empty,
	chest,
	enemy,
	outer
}

public class PCGBoardManager : MonoBehaviour
{
	
	//Helper classes Count and Pathtile
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

	[Serializable]
	public class PathTile
	{
		public TileType type;
		public Vector2 position;
		public List<Vector2> adjacentPathTiles;

		//Pathtile constructor saves the type, position and the adjacent tiles
		public PathTile (TileType t, Vector2 p, int min, int max, Dictionary<Vector2, TileType> currentTiles)
		{
			type = t;
			position = p;
			adjacentPathTiles = getAdjacentPath (min, max, currentTiles);
		}

		//the adjacent tiles get saved in ea tiles own List
		//had to be added with a "O" condition for better performance on outer walls
		public List<Vector2> getAdjacentPath (int minBound, int maxBound, Dictionary<Vector2, TileType> currentTiles)
		{
			List<Vector2> pathTiles = new List<Vector2> ();
			if (type != TileType.outer) {
				
				if (position.y + 1 < maxBound && !currentTiles.ContainsKey (new Vector2 (position.x, position.y + 1))) {
					pathTiles.Add (new Vector2 (position.x, position.y + 1));
				}
				if (position.x + 1 < maxBound && !currentTiles.ContainsKey (new Vector2 (position.x + 1, position.y))) {
					pathTiles.Add (new Vector2 (position.x + 1, position.y));
				}
				if (position.y - 1 > minBound && !currentTiles.ContainsKey (new Vector2 (position.x, position.y - 1))) {
					pathTiles.Add (new Vector2 (position.x, position.y - 1));
				}
				// the essential path is restricted in movement to prevent moving backwards
				if (position.x - 1 >= minBound && !currentTiles.ContainsKey (new Vector2 (position.x - 1, position.y)) && type != TileType.essential) {
					pathTiles.Add (new Vector2 (position.x - 1, position.y));
				}

			} else {
				//Outer walls need to surround the min and maxbound
				if (position.y + 1 <= maxBound + 1 && !currentTiles.ContainsKey (new Vector2 (position.x, position.y + 1))) {
					pathTiles.Add (new Vector2 (position.x, position.y + 1));
				}
				if (position.x + 1 <= maxBound + 1 && !currentTiles.ContainsKey (new Vector2 (position.x + 1, position.y))) {
					pathTiles.Add (new Vector2 (position.x + 1, position.y));
				}
				if (position.y - 1 >= minBound - 1 && !currentTiles.ContainsKey (new Vector2 (position.x, position.y - 1))) {
					pathTiles.Add (new Vector2 (position.x, position.y - 1));
				}
				if (position.x - 1 >= minBound - 1 && !currentTiles.ContainsKey (new Vector2 (position.x - 1, position.y))) {
					pathTiles.Add (new Vector2 (position.x - 1, position.y));
				}
			}
			return pathTiles;
		}
	}


	public int columns = 5;
	public int rows = 5;

	public GameObject exit;
	public int minBound = 0, maxBound;

	public static Vector2 startPos;
	public Vector2 endPos;

	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] outerWallTiles;
	public GameObject chestTile;

	//public GameObject enemy;
	public GameObject player;
	public GameObject playerInstance;

	private Transform boardHolder;
	private Dictionary<Vector2, TileType> gridPositions = new Dictionary<Vector2, TileType> ();
	private Dictionary<Vector2, TileType> outerWallPositions = new Dictionary<Vector2, TileType> ();



	public void StartDungeon ()
	{
		//This is the seed 
		//Random.InitState (1);
		gridPositions.Clear ();
		outerWallPositions.Clear ();
		maxBound = Random.Range (50, 101);

		BuildEssentialPath ();
		BuildRandomPath ();

		SetDungeonBoard (gridPositions, maxBound, endPos);
	}


	public void SetDungeonBoard (Dictionary<Vector2,TileType> dungeonTiles, int bound, Vector2 endPos)
	{
		boardHolder = new GameObject ("Board").transform;
		boardHolder.transform.SetParent (this.gameObject.transform);

		GameObject toInstantiate, instance;

		foreach (KeyValuePair<Vector2,TileType> tile in dungeonTiles) {
			toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
			instance = Instantiate (toInstantiate, new Vector3 (tile.Key.x, tile.Key.y, 0f), Quaternion.identity) as GameObject;
			instance.transform.SetParent (boardHolder);
			//if the tile includes a chest
			if (tile.Value == TileType.chest) {
				toInstantiate = chestTile;
				instance = Instantiate (toInstantiate, new Vector3 (tile.Key.x, tile.Key.y), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}

		//BuildOuterWalls ();
		//Bad performance -> runs through the complete dungeon to fill with walls


		//Creates the outer walls
		for (int x = -1; x < bound + 1; x++) {
			for (int y = -1; y < bound + 1; y++) {
				if (!dungeonTiles.ContainsKey (new Vector2 (x, y))) {
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (boardHolder);
				}
			}
		}



		toInstantiate = exit;
		instance = Instantiate (toInstantiate, new Vector3 (endPos.x, endPos.y, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (boardHolder);

		//Add the player instance into the board
		playerInstance = Instantiate (player, new Vector3 (startPos.x, startPos.y, 0), Quaternion.identity) as GameObject;
		playerInstance.transform.SetParent (boardHolder);
	}


	private void BuildEssentialPath ()
	{
		int randomY = Random.Range (0, maxBound + 1);
		PathTile ePath = new PathTile (TileType.essential, new Vector2 (0, randomY), minBound, maxBound, gridPositions);
		startPos = ePath.position;

		int boundTracker = 0;

		while (boundTracker < maxBound) {

			gridPositions.Add (ePath.position, ePath.type);

			int adjacentTileCount = ePath.adjacentPathTiles.Count;

			int randomIndex = Random.Range (0, adjacentTileCount);

			Vector2 nextEPathPos;
			if (adjacentTileCount > 0) {
				nextEPathPos = ePath.adjacentPathTiles [randomIndex];
			} else {
				break;
			}
			//Randomisation in the condition to determine the end position even before the maxBound is reached
			PathTile nextEPath = new PathTile (TileType.essential, nextEPathPos, minBound, maxBound, gridPositions);
			if (nextEPath.position.x > ePath.position.x || (nextEPath.position.x == maxBound - 1 && Random.Range (0, 2) == 1)) { // Update boundtracker before EPath update
				++boundTracker;
			} 

			ePath = nextEPath;
		}

		if (!gridPositions.ContainsKey (ePath.position))
			gridPositions.Add (ePath.position, ePath.type);

		endPos = new Vector2 (ePath.position.x, ePath.position.y);

	}

	private void BuildRandomPath ()
	{

		List<PathTile> pathQueue = new List<PathTile> ();
		foreach (KeyValuePair<Vector2,TileType> tile in gridPositions) {
			Vector2 tilePos = new Vector2 (tile.Key.x, tile.Key.y);
			//Random paths being created from the essential path may move to the left
			pathQueue.Add (new PathTile (TileType.random, tilePos, minBound, maxBound, gridPositions));
		}

		//For each tile in the Queue
		pathQueue.ForEach (delegate (PathTile tile) {

			int adjacentTileCount = tile.adjacentPathTiles.Count;
			if (adjacentTileCount != 0) {
				if (Random.Range (0, 5) == 1) {
					BuildRandomChamber (tile);
				} else if (Random.Range (0, 5) == 1 || (tile.type == TileType.random && adjacentTileCount > 1)) {

					int randomIndex = Random.Range (0, adjacentTileCount);

					Vector2 newRPathPos = tile.adjacentPathTiles [randomIndex];

					if (!gridPositions.ContainsKey (newRPathPos)) {
						gridPositions.Add (newRPathPos, TileType.random);

						PathTile newRPath = new PathTile (TileType.random, newRPathPos, minBound, maxBound, gridPositions);
						pathQueue.Add (newRPath);
					}
				}
			}
		});

		pathQueue.Clear ();
	}

	//Also adds a chest randomly
	private void BuildRandomChamber (PathTile tile)
	{
		int chamberSize = 3,
		adjacentTileCount = tile.adjacentPathTiles.Count,
		randomIndex = Random.Range (0, adjacentTileCount);
		Vector2 chamberOrigin = tile.adjacentPathTiles [randomIndex];

		for (int x = (int)chamberOrigin.x; x < chamberOrigin.x + chamberSize; x++) {
			for (int y = (int)chamberOrigin.y; y < chamberOrigin.y + chamberSize; y++) {
				Vector2 chamberTilePos = new Vector2 (x, y);
				if (!gridPositions.ContainsKey (chamberTilePos) &&
				    chamberTilePos.x < maxBound && chamberTilePos.x > 0 &&
				    chamberTilePos.y < maxBound && chamberTilePos.y > 0)
					//chance to create a chest 
				if (Random.Range (0, 60) == 1) {
					gridPositions.Add (chamberTilePos, TileType.chest);
				} else {
					gridPositions.Add (chamberTilePos, TileType.random);
				}
			}
		}
	}

	private void BuildOuterWalls ()
	{
		List<PathTile> allTiles = new List<PathTile> ();
		foreach (KeyValuePair<Vector2, TileType> tile in gridPositions) {
			Vector2 tilePos = new Vector2 (tile.Key.x, tile.Key.y);
			allTiles.Add (new PathTile (TileType.outer, tilePos, minBound, maxBound, gridPositions));
		}

		foreach (PathTile tile in allTiles) {
			int adjacentTileCount = tile.adjacentPathTiles.Count;
			if (adjacentTileCount != 0) {
				for (int i = 0; i < adjacentTileCount; i++) {
					Vector2 newOuterWallPos = tile.adjacentPathTiles [i];
					if (!gridPositions.ContainsKey (newOuterWallPos) && !outerWallPositions.ContainsKey (newOuterWallPos)) {
						outerWallPositions.Add (newOuterWallPos, TileType.outer);
						GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
						GameObject instance = Instantiate (toInstantiate, new Vector3 (newOuterWallPos.x, newOuterWallPos.y, 0), Quaternion.identity) as GameObject;
						instance.transform.SetParent (boardHolder);
					}
				}
			}
		}


		allTiles.Clear ();
	}
}










//Outdated code for creating a random infinite dungeon
/*
	 * 
	 * 
	 * 
	 * 
	 * private void addTiles (Vector2 tileToAdd)
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
	 * 
	 * 
	 * */


