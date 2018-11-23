using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float speed = 3;
	public bool canMove;

	Vector3 _initPosition;
	Vector3 _targetPosition;
	float _swapTutorialAnimationCurrentTime = 0 ;//this is the unscaledTime to lerp from curr position to target, for the swap tutorial.
	float _initSwapAnimTime;
	bool _startSwapTutorial;
	float _yPos = 35f;


	void Start () 
	{
		var gameMan = FindObjectOfType<GameManager> ();
		if (gameMan.CurrentLevelInfo.id == 0)
			canMove = false;
	}

	void Update ()
	{
		if (canMove)
		{
			CheckFingers();
			CheckButtons();	
		}
		
		CameraAnimationLerpForTutorial ();
	}


	public void StartSwapTutorial(Vector3 screenPoint, float timeShowingTuto)
	{
		canMove = false;
		_startSwapTutorial = true;
		_initPosition = transform.position;
		var worldPos = Camera.main.ScreenToWorldPoint (screenPoint);
		var exactPos = new Vector3 (worldPos.x, _yPos, worldPos.z);
		var dir = (exactPos - transform.position).normalized;
		var tuSum = exactPos + (transform.forward * -1 * 35);
		Debug.Log (tuSum);
		_targetPosition = tuSum;
		_initSwapAnimTime = timeShowingTuto;

	}

	void CameraAnimationLerpForTutorial ()
	{
		if (!_startSwapTutorial)
			return;

		_swapTutorialAnimationCurrentTime += Time.unscaledDeltaTime / _initSwapAnimTime;
		transform.position = Vector3.Lerp (_initPosition, _targetPosition, _swapTutorialAnimationCurrentTime);
		if (Mathf.Abs (Vector3.Distance (transform.position, _targetPosition)) < 0.2f) 
		{
			_swapTutorialAnimationCurrentTime = 0;
			_startSwapTutorial = false;
			canMove = true;
		}
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
