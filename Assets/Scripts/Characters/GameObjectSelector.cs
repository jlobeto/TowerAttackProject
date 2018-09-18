using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be for the user interaction with gameobjecs in the scene (minions, towers, maybe start nodes)
/// </summary>
public class GameObjectSelector : MonoBehaviour
{
    public bool isActivated;

	void Start () {
    }
	
	
	void Update ()
    {
        
	}

    public GameObject SelectGameObject(int layer)
    {
        if (!isActivated)
        {
            Debug.LogError("GameObjectSelector is disabled!");
            return null;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor")))
        {
            var overlaps = Physics.OverlapSphere(hit.point, 3.5f, 1 << layer);
            if (overlaps.Length > 0)
                return overlaps[0].gameObject;

            if (Physics.Raycast(ray, out hit, 100, 1 << layer))
            {
                return hit.collider.gameObject;
            }
        }
        
        return null;
    }


    private void OnDrawGizmos()
    {
        
    }
}
