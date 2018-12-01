using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroshockWire : MonoBehaviour {

    public bool toPath;

    Material _mat;
    Color _activateColor = Color.red;

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
