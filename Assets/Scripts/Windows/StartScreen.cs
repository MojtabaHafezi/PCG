using UnityEngine;
using System.Collections;

public class StartScreen : BaseScreen {

	public void Continue () {
		Debug.Log ("Continue button pressed");
	}

	public void Options () {
		Debug.Log ("Options button pressed");
	}

	public void NewGame () {
		//screenManager.Open (1);
		OnNextScreen ();
	}
		
}
