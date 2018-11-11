using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvasTutorial : MonoBehaviour
{
    List<Image> _arrowImages = new List<Image>();

	void Awake ()
    {
        _arrowImages = GetComponentsInChildren<Image>().ToList();
        DisableAllArrows();
    }
	
    public void EnableArrowByName(string name)
    {
        foreach (var item in _arrowImages)
        {
            if (item.gameObject.name == name)
            {
                item.enabled = true;
                break;
            }
                
        }
    }

	public void DisableArrowByName(string name)
	{
		foreach (var item in _arrowImages)
		{
			if (item.gameObject.name == name)
			{
				item.enabled = false;
				break;
			}
		}
	}

    public void DisableAllArrows()
    {
        foreach (var item in _arrowImages)
        {
            item.enabled = false;
        }
    }

    public void SetArrowPosition(Vector3 pos, string name)
    {
        foreach (var item in _arrowImages)
        {
            if (item.gameObject.name == name)
            {
                item.rectTransform.parent.position = new Vector3(pos.x, pos.y + 40, 0); //beatifull hardcoded
                break;
            }
        }
    }

	void Update () {
		
	}
}
