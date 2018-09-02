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
    RectTransform _rectTransform;
    float _distToRemove;

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
        if (transform.parent.GetInstanceID() == _startParent.GetInstanceID())
        {
            var dist = Vector2.Distance(transform.position, transform.parent.position);
            if (dist < _distToRemove)
                transform.position = _startPosition;
            else
                slotParent.OnRemoveMe();
        }
            
    }

    void Start () {
        _canvasGroup = GetComponent<CanvasGroup>();
        slotParent = GetComponentInParent<DragAndDropSlot>();

        _distToRemove = slotParent.childSize.y * 0.85f;
    }
	
	void Update () {
        //TODO/// Change this fucking shit.
		if(slotParent.GetInstanceID() != transform.parent.GetInstanceID())
            slotParent = GetComponentInParent<DragAndDropSlot>();
    }
}
