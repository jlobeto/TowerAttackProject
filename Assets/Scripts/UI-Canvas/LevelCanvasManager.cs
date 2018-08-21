using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvasManager : MonoBehaviour
{
    public Button skillButtonPrefab;

    public void CreateSkillButton(string name, Action onClick)
    {
        var btn = Instantiate<Button>(skillButtonPrefab, transform);
        btn.GetComponentInChildren<Text>().text = name;
        btn.onClick.AddListener(() => onClick());
    }

	void Start () {
		
	}
	
	void Update () {
		
	}
}
