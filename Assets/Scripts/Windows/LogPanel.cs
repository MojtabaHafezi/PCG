using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPanel : MonoBehaviour
{
	public GameObject Log;

	void Awake ()
	{
		Log.SetActive (false);

	}

	public void ShowLog ()
	{
		Log.SetActive (!Log.activeSelf);
	}
}
