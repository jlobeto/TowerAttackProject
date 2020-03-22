using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float speed = 3;
	public bool canMove;
    [Range(.5f,10)]
    public float perspectiveZoomSpeed = 0.5f;// The rate of change of the field of view in perspective mode.
    public Transform constraintA;
    public Transform constraintB;

    Vector3 _initPosition;
	Vector3 _targetPosition;
	float _swapTutorialAnimationCurrentTime = 0 ;//this is the unscaledTime to lerp from curr position to target, for the swap tutorial.
	float _initTimeShowingTuto;
	bool _startTutorialMovement;
	float _yPos = 35f;
    float _baseSpeed;
    Action<GameObject> _onFinishTutoMove;
    GameManager _gm;

	void Start () 
	{
        _gm = FindObjectOfType<GameManager> ();
		if (_gm.CurrentLevelInfo.id == 0)
			canMove = false;

        _baseSpeed = speed;

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

        LimitCameraMovement();
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
                LimitCameraMovement();
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
            
            Camera.main.fieldOfView += deltaMagnitudeDiff * (perspectiveZoomSpeed/100);
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30f, 90f);

            float magicNumber = 0;
            if (Camera.main.fieldOfView < 40)
                magicNumber = 10;

            speed = _baseSpeed * ((Camera.main.fieldOfView - magicNumber) / 100);
        }
    }

    void LimitCameraMovement()
    {
        if (constraintA == null || constraintB == null) return;
        
        var xClamp = Mathf.Clamp(transform.position.x, constraintB.position.x, constraintA.position.x);
        var zClamp = Mathf.Clamp(transform.position.z, constraintB.position.z, constraintA.position.z);
        var clampedPos = new Vector3(xClamp, transform.position.y, zClamp);

        transform.position = clampedPos;
    }

    private void OnDrawGizmos()
    {
        if (constraintA == null || constraintB == null) return;
        Gizmos.color = Color.yellow;

        var x_a = constraintA.position.x;
        var x_b = constraintB.position.x;
        var z_a = constraintA.position.z;
        var z_b = constraintB.position.z;

        Gizmos.DrawLine(new Vector3(x_a, 0, z_a), new Vector3(x_a, 0, z_b));
        Gizmos.DrawLine(new Vector3(x_a, 0, z_a), new Vector3(x_b, 0, z_a));

        Gizmos.DrawLine(new Vector3(x_b, 0, z_b), new Vector3(x_a, 0, z_b));
        Gizmos.DrawLine(new Vector3(x_b, 0, z_b), new Vector3(x_b, 0, z_a));
    }
}
