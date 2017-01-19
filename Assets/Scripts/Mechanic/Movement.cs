

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
	public float moveTime = 0.1f;
	//Time it will take object to move, in seconds.

	private Rigidbody2D rb2D;
	//The Rigidbody2D component attached to this object.
	private float inverseMoveTime;
	//Used to make movement more efficient.


	//Protected, virtual functions can be overridden by inheriting classes.
	protected virtual void Start ()
	{

		//Get a component reference to this object's Rigidbody2D
		rb2D = GetComponent <Rigidbody2D> ();

		//By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
		inverseMoveTime = 1f / moveTime;
	}



	//Move takes parameters for x direction, y direction
	protected void Move (int xDir, int yDir)
	{
		//Store start position to move from, based on objects current transform position.
		Vector2 start = transform.position;

		// Calculate end position based on the direction parameters passed in when calling Move.
		Vector2 end = start + new Vector2 (xDir, yDir);


		//Find a new position proportionally closer to the end, based on the moveTime
		Vector3 newPostion = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);

		//Call MovePosition on attached Rigidbody2D and move it to the calculated position.
		rb2D.MovePosition (newPostion);

	

	
	}


	//Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
	protected IEnumerator SmoothMovement (Vector3 end)
	{
		//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
		//Square magnitude is used instead of magnitude because it's computationally cheaper.
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		//While that distance is greater than a very small amount (Epsilon, almost zero):
		while (sqrRemainingDistance > 0) {
			

			//Recalculate the remaining distance after moving.
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;

			//Return and loop until sqrRemainingDistance is close enough to zero to end the function
			yield return null;
		}
	}






}