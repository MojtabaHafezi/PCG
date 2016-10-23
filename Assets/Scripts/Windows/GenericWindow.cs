﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class GenericWindow : MonoBehaviour {
	public GameObject firstSelected;

	protected EventSystem eventSystem {
		get{ return GameObject.Find ("EventSystem").GetComponent<EventSystem> (); }
	}

	public void OnFocus () {
		eventSystem.SetSelectedGameObject (firstSelected);
	}

	public void Open () {
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
	

}