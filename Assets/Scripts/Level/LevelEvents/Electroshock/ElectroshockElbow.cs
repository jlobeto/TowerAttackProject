using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroshockElbow : MonoBehaviour
{
    public HitAreaCollider effectZone;
    public List<ElectroshockElectricity> electricityFeedbacks;
    public List<ElectroshockWire> wires;

    /// <summary>
    /// When the user press this elbow, this will turn into true, if true, electroshock won't affecct the path.
    /// </summary>
    bool _disabledByUser = false;
    Material _material;
    Color _activateColor = Color.red;
    Collider _myCollider;

    int _minionAmountInsideAOE;
    Vector3 _initPosOfAOE;//Used to emulate a triggerExit.

    public bool IsDisabledByUser { get { return _disabledByUser; } }

	void Start ()
    {
        _material = GetComponentInChildren<MeshRenderer>().material;
        _material.EnableKeyword("_EMISSION");
        _material.SetFloat("EmissionQty", 0f);
        _myCollider = GetComponent<Collider>();
        _myCollider.enabled = false;

        effectZone.thisCollider.enabled = false;
        effectZone.OnTriggerEnterCallback += OnEnterZone;
        effectZone.OnTriggerExitCallback += OnExitZone;

        _initPosOfAOE = effectZone.transform.position;
    }

    void OnEnterZone(Collider col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Minion")) return;
        //Debug.Log("on enter zone");

        var m = col.GetComponent<Minion>();

        if (m.minionType == MinionType.MiniZeppelin) return;

        m.MainSkill.GetElectroshock(true);
    }

    void OnExitZone(Collider col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Minion")) return;
        //Debug.Log("on exit zone");

        var m = col.GetComponent<Minion>();

        if (m.minionType == MinionType.MiniZeppelin) return;

        m.MainSkill.GetElectroshock(false);
    }

    void Update ()
    {
	    	
	}
    

    /// <summary>
    /// Change color and enable trigger to be 'touchable'
    /// </summary>
    public void RiceEvent()
    {
        _material.SetColor("_EmissionColor", _activateColor);
        SetWireColors(true);
        _myCollider.enabled = true;
        _disabledByUser = false;
    }

    /// <summary>
    /// this will make the elbow not to throw electroshock to his path.
    /// This will be triggered by the user interaction or the timer of electroshockManager
    /// </summary>
    public void DisableEvent()
    {
        _myCollider.enabled = false;
        _material.SetColor("_EmissionColor", Color.black);

        foreach (var item in wires)
        {
            if (item.toPath)
                item.SetWireColor(false);
        }

        _disabledByUser = true;
        effectZone.thisCollider.enabled = false;
    }

    /// <summary>
    /// Activate The electroshock on the path
    /// </summary>
    /// <param name="time">the duration of the electroshock</param>
    public void ActivateElectroshock(float time)
    {
        //if this elbow hasn't been deactivated by user enable the effectzone
        effectZone.thisCollider.enabled = !_disabledByUser;

        if(effectZone.thisCollider.enabled)
        {
            effectZone.transform.position = _initPosOfAOE;
            foreach (var item in electricityFeedbacks)
            {
                item.EnableElectricity();
            }
        }

        StartCoroutine(DeactivatingElectroshock(time));
    }

    IEnumerator DeactivatingElectroshock(float t)
    {
        yield return new WaitForSeconds(t);

        //To not lose ontriggerexit i must position the hitareacollider in la concha del mono.
        effectZone.transform.position = new Vector3(1000, 1000, 1000);
        _material.SetColor("_EmissionColor", Color.black);
        SetWireColors(false);

        foreach (var item in electricityFeedbacks)
        {
            item.DisableElectricity();
        }
    }

    void SetWireColors(bool activate)
    {
        foreach (var item in wires)
        {
            item.SetWireColor(activate);
        }
    }
}
