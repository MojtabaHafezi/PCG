using UnityEngine;
using System.Collections;

public class StartScreen : BaseScreen {

	public void Continue () {
		//TODO: LOAD GAME PREFS
		screenManager.Open((int)Screens.TownScreen);
	}

	public void Exit () {
		Debug.Log ("Exit button pressed");
	}

	public void NewGame () {
		//TODO: NEW GAME PREFS
		screenManager.Open((int)Screens.TownScreen);
	}

    

    void Start()
    {
        Open();
    }

}
