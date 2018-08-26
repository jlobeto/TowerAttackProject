using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler , IEndDragHandler, IDragHandler
{
    public static DragHandler itemBeingDragged;
    public MinionType minionType;
    public DragAndDropSlot slotParent;
    Vector3 _startPosition;
    Transform _startParent;
    CanvasGroup _canvasGroup;

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = this;
        _startPosition = transform.position;
        _startParent = transform.parent;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        _canvasGroup.blocksRaycasts = true;
        if (transform.parent == _startParent)
            transform.position = _startPosition;
    }

    void Start () {
        _canvasGroup = GetComponent<CanvasGroup>();
        slotParent = GetComponentInParent<DragAndDropSlot>();
    }
	
	void Update () {
        //TODO/// Change this fucking shit.
		if(slotParent != transform.parent)
            slotParent = GetComponentInParent<DragAndDropSlot>();
    }
}
