using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be for the user interaction with gameobjecs in the scene (minions, towers, maybe start nodes)
/// </summary>
public class GameObjectSelector : MonoBehaviour
{
	Collider[] _minionsOverlap;
	void Start () {
    }
	
	
	void Update ()
    {
        
	}

    public GameObject SelectGameObject(int layer)
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor")))
        {
            var overlaps = Physics.OverlapSphere(hit.point, 1.5f, 1 << layer);
            if (overlaps.Length > 0)
                return overlaps[0].gameObject;

            if (Physics.Raycast(ray, out hit, 100, 1 << layer))
            {
                return hit.collider.gameObject;
            }
        }
        
        return null;
    }

	public List<Minion> GetMinionsSelection(Vector3 position , float radius)
	{
		var point2 = new Vector3 (position.x, position.y + 6, position.z);
		_minionsOverlap = Physics.OverlapCapsule(position, point2, radius, 1 << LayerMask.NameToLayer("Minion"));
		if (_minionsOverlap.Length > 0)
			return _minionsOverlap.Select(i => i.GetComponent<Minion>()).ToList();
		
		return new List<Minion>();
	}


}
