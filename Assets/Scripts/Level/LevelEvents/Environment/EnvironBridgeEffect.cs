using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironBridgeEffect : MonoBehaviour
{
    public string destination = "";

    List<MeshRenderer> _renderers = new List<MeshRenderer>();
    float _toHide;
    float _initHide;
    bool _hide;
    float _discountCounter = 1;
    bool _intermitent;

    void Start()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>().ToList();
    }

    public void StartHidding(float timeToHide)
    {
        _initHide = timeToHide;
        _toHide = timeToHide;
        _hide = true;
    }

    public void Show()
    {
        affectMesh(true);
    }

    void Update()
    {
        if (!_hide) return;

        _toHide -= Time.deltaTime;
        if (_toHide < 0)
        {
            affectMesh(false);
            _hide = false;
        }
    }

    public void affectMesh(bool visible)
    {
        foreach (var item in _renderers)
        {
            item.enabled = visible;
        }
    }
}
