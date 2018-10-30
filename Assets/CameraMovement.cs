using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 3;
	
	void Start () {
		
	}
	
	
	void Update ()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var zoom = new Vector3(x, 0, z);
        transform.position += Time.deltaTime * speed * zoom * 5; 
    }
}
