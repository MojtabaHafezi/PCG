using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Focus : MonoBehaviour
{
	public GameObject firstSelected;


	protected EventSystem eventSystem {
		get{ return GameObject.Find ("EventSystem").GetComponent<EventSystem> (); }
	}

	public void OnFocus ()
	{
		eventSystem.SetSelectedGameObject (firstSelected);
	}
	// Use this for initialization
	void Start ()
	{
		OnFocus ();
	}


	// Update is called once per frame
	void Update ()
	{
		
	}
}
