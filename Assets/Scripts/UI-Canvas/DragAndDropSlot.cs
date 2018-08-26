using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropSlot : MonoBehaviour, IDropHandler
{
    public int index;

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
            var myChild = GetComponentInChildren<DragHandler>();
            if (myChild.minionType == DragHandler.itemBeingDragged.minionType) return;

            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject
                , null
                , (x, y) => x.HasChanged
                (this
                , myChild
                , DragHandler.itemBeingDragged.slotParent
                , DragHandler.itemBeingDragged));
        }
    }

    void Start () {
		
	}
	
	
	void Update () {
		
	}
}
