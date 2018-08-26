using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropSlot : MonoBehaviour, IDropHandler
{

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    
    public void OnDrop(PointerEventData eventData)
    {
        if (item != null)
        {
            var dragHandler = GetComponentInChildren<DragHandler>();
            if (dragHandler.minionType == DragHandler.itemBeingDragged.minionType) return;

            dragHandler.transform.SetParent(DragHandler.itemBeingDragged.slotParent.transform);
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
    
    void Start () {
		
	}
	
	
	void Update () {
		
	}
}
