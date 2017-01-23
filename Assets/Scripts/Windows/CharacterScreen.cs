using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScreen : BaseScreen
{
	public enum Location
	{
		Town,
		Dungeon,
		PCGDungeon

	}

	private static Location currentLocation;

	public static void SetLocation (Location location)
	{
		currentLocation = location;
	}

	public void BackToPrevious ()
	{
		if (currentLocation == Location.Town) {
			BackToTown ();
		}
		if (currentLocation == Location.Dungeon) {
			BackToDungeon ();
		}
		if (currentLocation == Location.PCGDungeon) {
			BackToPCGDungeon ();
		}
	}

	private void BackToTown ()
	{
		screenManager.Open ((int)Screens.TownScreen);
	}

	private void BackToDungeon ()
	{
		screenManager.Open ((int)Screens.DungeonScreen);
	}

	private void BackToPCGDungeon ()
	{
		screenManager.Open ((int)Screens.PCGDungeonScreen);
	}
}
