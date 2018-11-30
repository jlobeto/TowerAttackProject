using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float speed = 3;
	public bool canMove;
    public float perspectiveZoomSpeed = 0.5f;// The rate of change of the field of view in perspective mode.

    Vector3 _initPosition;
	Vector3 _targetPosition;
	float _swapTutorialAnimationCurrentTime = 0 ;//this is the unscaledTime to lerp from curr position to target, for the swap tutorial.
	float _initTimeShowingTuto;
	bool _startTutorialMovement;
	float _yPos = 35f;
    Action<GameObject> _onFinishTutoMove;
    GameManager _gm;

	void Start () 
	{
        _gm = FindObjectOfType<GameManager> ();
		if (_gm.CurrentLevelInfo.id == 0)
			canMove = false;
	}

	void Update ()
	{
		if (canMove)
		{
			CheckFingers();
			CheckButtons();
            Zooming();

        }
		
		CameraAnimationLerpForTutorial ();
	}

    public void SetCameraMovement(bool value)
    {
        if(value && _gm.CurrentLevelInfo.id == 0)
        {
            return;
        }
        canMove = value;
    }

	public void StartCameraMoveForTutorial(Vector3 screenPoint, float timeShowingTuto, Action<GameObject> func = null)
	{
		canMove = false;
		_startTutorialMovement = true;
		_initPosition = transform.position;
		var worldPos = Camera.main.ScreenToWorldPoint (screenPoint);
		var exactPos = new Vector3 (worldPos.x, _yPos, worldPos.z);
		var dir = (exactPos - transform.position).normalized;
		var tuSum = exactPos + (transform.forward * -1 * 35);
		//Debug.Log (tuSum);
		_targetPosition = tuSum;
		_initTimeShowingTuto = timeShowingTuto;
        _onFinishTutoMove = func;

    }

	void CameraAnimationLerpForTutorial ()
	{
		if (!_startTutorialMovement)
			return;

		_swapTutorialAnimationCurrentTime += Time.unscaledDeltaTime / _initTimeShowingTuto;
		transform.position = Vector3.Lerp (_initPosition, _targetPosition, _swapTutorialAnimationCurrentTime);
		if (Mathf.Abs (Vector3.Distance (transform.position, _targetPosition)) < 0.2f) 
		{
			_swapTutorialAnimationCurrentTime = 0;
			_startTutorialMovement = false;
			canMove = true;

            if (_onFinishTutoMove != null)
                _onFinishTutoMove(null);

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
        else if(Input.touchCount == 2)
        {
            Zooming();
        }
    }

    void Zooming()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            
            // Otherwise change the field of view based on the change in distance between the touches.
            Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

            // Clamp the field of view to make sure it's between 0 and 180.
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30f, 90f);
        }
    }

}
