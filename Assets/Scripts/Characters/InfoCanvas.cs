using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This shit will be inside the gamobject that is above the character head.
/// </summary>
public class InfoCanvas : MonoBehaviour
{
    public Image hpBar;

    float initLife;

    public void Init(float life)
    {
        initLife = life;
    }

    public void UpdateLife(float newLife)
    {
        hpBar.fillAmount = newLife / initLife;
    }

	void Start () {
		
	}
	
	void Update ()
    {
        var dir = (Camera.main.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir * -1);
    }
}
