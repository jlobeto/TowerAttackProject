using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour
{

    public GridLayoutGroup grid;
    public Text title;
    public WorldPadlock padlockUI;

    
    int _worldId;

	void Awake ()
    {
	    	
	}

    public void Init(int id, bool worldUnlocked, int neededToUnlock)
    {
        _worldId = id;
        title.text = "WORLD " + (id+1);
        padlockUI.SetLockUI(worldUnlocked, neededToUnlock);
        DeleteGridChildren();
    }

    /// <summary>
    /// Because the grid will be created using an already existing grid.
    /// </summary>
    public void DeleteGridChildren()
    {
        foreach (Transform item in grid.transform)
        {
            Destroy(item.gameObject);
        }
    }

    void Update () {
		
	}
}
