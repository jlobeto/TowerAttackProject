
using UnityEngine;
using UnityEngine.EventSystems;

public interface IHasChanged : IEventSystemHandler
{
    void HasChanged(DragAndDropSlot affectedSlot, DragHandler affectedDraggable, DragAndDropSlot fromSlot, DragHandler beingDragged);
}