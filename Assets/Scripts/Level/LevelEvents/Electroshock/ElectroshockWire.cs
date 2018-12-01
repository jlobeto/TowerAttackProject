using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroshockWire : MonoBehaviour {

    public bool toPath;

    Material _mat;
    Color _activateColor = new Color(0.1f, 0.46f, 0.74f);

    void Start()
    {
        _mat = GetComponentInChildren<MeshRenderer>().material;
        _mat.EnableKeyword("_EMISSION");
    }

    public void SetWireColor(bool activate)
    {
        var color = activate ? _activateColor : Color.black;
        _mat.SetColor("_EmissionColor", color);
    }

}
