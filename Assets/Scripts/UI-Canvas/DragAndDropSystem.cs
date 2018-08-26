using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropSystem : MonoBehaviour, IHasChanged
{
    public DragAndDropSlot slotPref;

    List<DragAndDropSlot> _slots = new List<DragAndDropSlot>();
    int _currentIndex = 0;
    LevelCanvasManager _canvas;

    public void AddSlot(MinionType t)
    {
        var slot = Instantiate<DragAndDropSlot>(slotPref, transform);
        slot.index = _currentIndex;
        slot.OnRemoveMe += (() => OnDeleteSlot(slot));
        _slots.Add(slot);

        GameObject slotImagePrefab = GetPrefab(t);
        var icon = Instantiate(slotImagePrefab, slot.transform);
        icon.GetComponent<DragHandler>().minionType = t;

        _currentIndex++;
    }

    void OnDeleteSlot(DragAndDropSlot slot)
    {
        _canvas.MinionDeleted(slot.index);
        _slots.Remove(slot);
        Destroy(slot.gameObject);
        ReorderSlotsIndeces();
    }

    public void HasChanged(DragAndDropSlot affectedSlot, DragHandler affectedDraggable, DragAndDropSlot fromSlot, DragHandler beingDragged)
    {
        SetParent(beingDragged.transform, affectedSlot.transform);
        SetParent(affectedDraggable.transform, fromSlot.transform);

        _canvas.MinionOrderUpdated(fromSlot.index, affectedSlot.index);
    }

    void SetParent(Transform child, Transform parent)
    {
        child.SetParent(parent);
    }


    void Start () {
        _canvas = GetComponentInParent<LevelCanvasManager>();
    }
	
	
	void Update () {
		
	}

    GameObject GetPrefab(MinionType t)
    {
        GameObject slotImagePrefab = null;
        switch (t)
        {
            case MinionType.Runner:
                slotImagePrefab = Resources.Load("UI/DragAndDropSprites/RunnerIcon", typeof(GameObject)) as GameObject;
                break;
            case MinionType.Tank:
                slotImagePrefab = Resources.Load("UI/DragAndDropSprites/TankIcon", typeof(GameObject)) as GameObject;
                break;
            case MinionType.Dove:
                slotImagePrefab = Resources.Load("UI/DragAndDropSprites/DroneIcon", typeof(GameObject)) as GameObject;
                break;
            case MinionType.Zeppelin:
                slotImagePrefab = Resources.Load("UI/DragAndDropSprites/ZepelinIcon", typeof(GameObject)) as GameObject;
                break;
            case MinionType.FatTank:
                break;
            case MinionType.GoldDigger:
                break;
            case MinionType.Healer:
                break;
            case MinionType.Ghost:
                break;
            case MinionType.WarScreammer:
                break;
            case MinionType.Eagle:
                break;
            case MinionType.Clown:
                break;
            default:
                break;
        }
        return slotImagePrefab;
    }

    void ReorderSlotsIndeces()
    {
        var index = 0;
        foreach (var item in _slots)
        {
            item.index = index;
            index++;
        }
    }
}
