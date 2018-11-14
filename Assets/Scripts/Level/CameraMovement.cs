using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float speed = 3;

	void Start () 
	{

	}


	void Update ()
	{
		CheckFingers();
		CheckButtons();
	}

	void CheckButtons()
	{
		var x = Input.GetAxis("Horizontal");
		var z = Input.GetAxis("Vertical");
		var zoom = new Vector3(x, 0, z);
		transform.position += Time.deltaTime * speed * zoom * 5;
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
				transform.Translate (new Vector3 (-deltaPos.x * (speed / 3), 0 , -deltaPos.y * (speed / 3)));
			}
		}
	}

}
