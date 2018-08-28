using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be for the user interaction with gameobjecs in the scene (minions, towers, maybe start nodes)
/// </summary>
public class GameObjectSelector : MonoBehaviour
{
    public bool isActivated;
    Level _level;

	void Start () {
        _level = FindObjectOfType<Level>();
    }
	
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var g = SelectGameObject(LayerMask.NameToLayer("Tower"));
            if(g != null)
                Debug.Log(g.name);
        }
	}
    
    public GameObject SelectGameObject(int layer)
    {
        Vector3 pos = Input.mousePosition;
        pos.z = Camera.main.transform.position.y;
        pos = Camera.main.ScreenToWorldPoint(pos);
        var ray = new Ray(pos, (pos - Camera.main.transform.position).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor")))
        {
            var overlaps = Physics.OverlapSphere(hit.point,5, 1 << layer);
            if (overlaps.Length > 0)
                return overlaps[0].gameObject;
        }
        return null;
    }


    private void OnDrawGizmos()
    {
        
    }
}
