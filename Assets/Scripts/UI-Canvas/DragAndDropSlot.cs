using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropSlot : MonoBehaviour, IDropHandler
{
    public int index;
    public Vector2 childSize;
    public Action OnRemoveMe = delegate { };

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
        if (item == null) return;
        
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


    void Start () {
        childSize = GetComponent<GridLayoutGroup>().cellSize;

    }
	
	
	void Update () {
		
	}
}
