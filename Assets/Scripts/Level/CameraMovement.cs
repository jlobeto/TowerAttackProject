using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float speed = 3;
	public bool canMove;


	void Start () 
	{
		var gameMan = FindObjectOfType<GameManager> ();
		if (gameMan.CurrentLevelInfo.id == 0)
			canMove = false;
	}

	void Update ()
	{
		if (!canMove)
			return;
		
		CheckFingers();
		CheckButtons();
	}

	void CheckButtons()
	{
		var x = Input.GetAxis("Horizontal");
		var z = Input.GetAxis("Vertical");
		transform.position += Time.deltaTime * speed * z * transform.forward * 10;
		transform.position += Time.deltaTime * speed * x * transform.right * 10;
	}

	void CheckFingers()
	{
		if (Input.touchCount == 1)
		{
			var t1 = Input.GetTouch (0);
			//var t2 = Input.GetTouch (1);

			if (t1.phase == TouchPhase.Moved)// && t2.phase == TouchPhase.Moved) 
			{
				var deltaPos = t1.deltaPosition;
				transform.Translate (new Vector3 (-deltaPos.x * (speed / 22), 0 , -deltaPos.y * (speed / 22)));
			}
		}
	}

}
