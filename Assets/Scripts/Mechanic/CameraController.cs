using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController instance = null;
	public GameObject player;
	public Transform root;

	private Vector3 offset;


	void Start ()
	{
		if (instance == null)
			instance = this;
		
		if (player != null)
			offset = transform.position - player.transform.position;
	}

	void LateUpdate ()
	{
		if (player != null)
			transform.position = player.transform.position + offset;
	}

	public void InDungeon ()
	{
		//player = GameObject.Find ("Player(Clone)");
		player = GameObject.FindGameObjectWithTag ("Player");

		if (player != null) {
			transform.SetParent (player.transform);
			transform.position = player.transform.position;
			offset = transform.position - player.transform.position;
		}

	}

	public void OutDungeon ()
	{
		instance.transform.parent = null;
		player = null;
		offset = new Vector3 (0, 0, 0);
		transform.position = new Vector3 (0f, 0f, 0f);
	}
}
