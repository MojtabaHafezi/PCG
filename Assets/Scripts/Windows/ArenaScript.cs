using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : BaseScreen {
	
	public void BackToTown() {
		screenManager.Open ((int)Screens.TownScreen);
	}

	public void ToGameOver() {
		screenManager.Open ((int)Screens.GameOverScreen);
	}
}
