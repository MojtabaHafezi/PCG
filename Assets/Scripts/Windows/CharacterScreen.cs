using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScreen : BaseScreen {
	private static bool inTown;

	public static bool GetInTown() {
		return inTown;
	}

	public static void SetInTown(bool value) {
		inTown = value;
	}

	public void BackToPrevious() {
		if (inTown) {
			BackToTown ();
		} else {
			BackToDungeon ();
		}
	}

	private void BackToTown() {
		screenManager.Open ((int)Screens.TownScreen);
	}

	private void BackToDungeon() {
		screenManager.Open ((int)Screens.DungeonScreen);
	}
}
