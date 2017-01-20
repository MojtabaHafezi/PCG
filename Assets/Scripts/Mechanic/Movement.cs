

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{

	public float speed = 2.0f;

	public Vector3 position;

	private Rigidbody2D rb2D;


	//Protected, virtual functions can be overridden by inheriting classes.
	protected virtual void Start ()
	{

		//Get a component reference to this object's Rigidbody2D
		rb2D = GetComponent <Rigidbody2D> ();

		position = transform.position;
	}
		

	//Move takes parameters for x direction, y direction
	protected void Move (int xDir, int yDir)
	{
		//Store start position to move from, based on objects current transform position.
		Vector3 start = transform.position;

		// Calculate end position based on the direction parameters passed in when calling Move.
		Vector3 end = start + new Vector3 (xDir, yDir, 0f);

		transform.position = Vector3.MoveTowards (start, end, speed * Time.deltaTime);
	
	}






}