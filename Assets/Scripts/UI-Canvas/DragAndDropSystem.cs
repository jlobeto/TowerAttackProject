using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropSystem : MonoBehaviour, IHasChanged
{
    public DragAndDropSlot slotPref;
    public void AddSlot(MinionType t)
    {
        var slot = Instantiate<DragAndDropSlot>(slotPref, transform);
        GameObject slotImagePrefab = GetPrefab(t);
        var icon = Instantiate(slotImagePrefab, slot.transform);
        icon.GetComponent<DragHandler>().minionType = t;
    }


    public void HasChanged()
    {
        Debug.Log("has changed");
    }

    void Start () {
		
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

}
