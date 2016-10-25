using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class BaseScreen : MonoBehaviour {
	
	public GameObject firstSelected;
	public Screens nextScreen;
	public Screens previousScreen;
	public static ScreenManager screenManager;

	protected EventSystem eventSystem {
		get{ return GameObject.Find ("EventSystem").GetComponent<EventSystem> (); }
	}

	public void OnFocus () {
		eventSystem.SetSelectedGameObject (firstSelected);
	}

	public virtual void Open () {
		Display (true);
		OnFocus ();
	}

	public void Close () {
		Display (false);
	}

	protected void Display (bool value) {
		gameObject.SetActive (value);
	}

	//need to override in child Class for activating instead of auto-closing the window
	protected virtual void Awake () {
		Close ();
	}

	public void OnNextScreen () {
		screenManager.Open ((int)nextScreen - 1);
	}

	public void OnPreveiousScreen () {
		screenManager.Open ((int)previousScreen - 1);
	}
}
