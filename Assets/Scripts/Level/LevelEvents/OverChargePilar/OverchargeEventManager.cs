using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargeEventManager : MonoBehaviour
{
    public int minionsAmount = 3;
    public float stunTime = 5f;
    [Header("How much time pilars will be activated")]
    public float activeTime = 5f;

    List<OverchargePilar> _pilars;
    Level _lvl;
    int _minionsAmountAux;
    bool _pilarsEnabled;

	void Start ()
    {
        _pilars = GetComponentsInChildren<OverchargePilar>().ToList();
        _minionsAmountAux = minionsAmount;
        _lvl = FindObjectOfType<Level>();
        _lvl.MinionManager.OnNewMinionSpawned += NewMinionSpawnedHandler;
        
    }


    void Update ()
    {
        if (!_pilarsEnabled) return;

        if (Input.GetMouseButtonUp(0))
        {
            var selected = _lvl.GameObjectSelector.SelectGameObject(LayerMask.NameToLayer("OverchargePilar"));
            if (selected == null) return;
            
            foreach (var item in _pilars)
            {
                if (item.gameObject.GetInstanceID() != selected.GetInstanceID())
                    continue;

                Debug.Log("pilar selected");

                item.StunTowers();
                StopCoroutine(WaitToDeactivatePilars());
                DisabledAllPilars();
                break;
            }
            
        }
    }

    void NewMinionSpawnedHandler(MinionType t)
    {
        _minionsAmountAux--;

        if (_minionsAmountAux > 0) return;

        Debug.Log("activating pilars");
        _pilarsEnabled = true;
        _minionsAmountAux = minionsAmount;
        foreach (var item in _pilars)
        {
            item.ActivatePilar(stunTime);
        }

        StartCoroutine(WaitToDeactivatePilars());
    }

    IEnumerator WaitToDeactivatePilars()
    {
        yield return new WaitForSeconds(activeTime);
        DisabledAllPilars();
    }

    void DisabledAllPilars()
    {
        Debug.Log("-deactivating pilars");

        _pilarsEnabled = false;

        foreach (var item in _pilars)
        {
            item.DeactivatePilar();
        }
    }
}



