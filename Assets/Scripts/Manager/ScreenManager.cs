using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {
	//TODO: Singleton pattern

	/*
	 * Array of screens, reference passed through unity interface
	 * 0 = StartScreen, 1 = GameOverScreen, 2 = GameScreen
	 */
	public BaseScreen[] screens;
	public int defaultScreen = 0;
	public int currentScreen;

	//return the screen in the array at given index
	public BaseScreen GetScreen (int value) {
		return screens [value];
	}

	/*
	 * Goes through all arrays and closes all but the one asked for
	 * 
	 */
	private void ToggleVisibility (int value) {
		var total = screens.Length;

		for (var i = 0; i < total; i++) {
			var screen = screens [i];
			if (i == value) {
				screen.Open ();
			} else if (screen.gameObject.activeSelf) {
				screen.Close ();
			}
		}
	}

	public BaseScreen Open (int value) {
		if (value < 0 || value >= screens.Length) {
			return null;
		}
		currentScreen = value;
		ToggleVisibility (currentScreen);
		return GetScreen (currentScreen);
	}

	void Start () {
		BaseScreen.screenManager = this;
		Open (defaultScreen);
	}
}
