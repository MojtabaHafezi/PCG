using UnityEngine;
using System.Collections;

public class GameOverScreen : BaseScreen {

	public void MainMenuPressed () {
		screenManager.Open ((int) Screens.StartScreen);
	}
}
