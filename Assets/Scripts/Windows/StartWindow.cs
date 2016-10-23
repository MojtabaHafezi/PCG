using UnityEngine;
using System.Collections;

public class StartWindow : GenericWindow {

	//removed the base.Awake()
	protected override void Awake () {
	}

	void Start () {
		Open ();
	}

	public void Continue () {
		Debug.Log ("Continue button pressed");
	}

	public void Options () {
		Debug.Log ("Options button pressed");
	}

	public void NewGame () {
		Debug.Log ("New Game button pressed");
	}
}
