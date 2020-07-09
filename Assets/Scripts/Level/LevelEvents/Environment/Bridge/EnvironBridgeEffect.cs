using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironBridgeEffect : MonoBehaviour
{
    public string destination = "";
    public HitAreaCollider hitAreaColIn;
    public HitAreaCollider hitAreaColOut;

    Animator _pathAnimator;
    float _timerToPlayNext = 0.3f;
    float _timerToPlayNextAux;
    bool _activate;
    int _pathIndexToAnimate;
    
    List<GroundMinion> _minionsOnBridge;


    void Start()
    {
        _pathAnimator = GetComponentInChildren<Animator>();
        //Debug.Log(destination + " ... paths count " + _paths.Count);
        _timerToPlayNextAux = _timerToPlayNext;

        hitAreaColIn.OnTriggerEnterCallback += OnTriggerEnterCol;
        hitAreaColOut.OnTriggerEnterCallback += OnTriggerExitCol;
        _minionsOnBridge = new List<GroundMinion>();
    }

    void OnTriggerEnterCol(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("MinionBridgeCol"))
        {
            _minionsOnBridge.Add(col.GetComponentInParent<GroundMinion>());
            
           //Debug.Log("Trigger Enter " + col.gameObject.GetInstanceID());
        }
    }

    void OnTriggerExitCol(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("MinionBridgeCol"))
        {
            var groundM = col.GetComponentInParent<GroundMinion>();
            if(_minionsOnBridge.Contains(groundM))
                _minionsOnBridge.Remove(groundM);

            //Debug.Log("Trigger Exit " + col.gameObject.GetInstanceID());
        }
    }

    public bool IsMinionInsideBridge(GroundMinion m)
    {
        foreach (var item in _minionsOnBridge)
        {
            if (item == null) continue;

            if (item.gameObject.GetInstanceID() == m.gameObject.GetInstanceID())
                return true;
        }

        return false;
    }


    void Update()
    {
        //if (!_activate) return;

        //if (_pathIndexToAnimate < _paths.Count)
        //{
        //    _timerToPlayNextAux -= Time.deltaTime;
        //    if (_timerToPlayNextAux < 0)
        //    {
        //        _paths[_pathIndexToAnimate].SetBool("startFall", true);
        //        _pathIndexToAnimate++;
        //        _timerToPlayNextAux = _timerToPlayNext;
        //    }
        //}
        //else
        //{
        //    _activate = false;
        //}
    }

    public void MakeAllWayDown()
    {
        _activate = true;
        _timerToPlayNextAux = _timerToPlayNext;
        _pathIndexToAnimate = 0;
        _pathAnimator.SetBool("open", true);
        _pathAnimator.SetBool("close", false);
        //_paths[_pathIndexToAnimate].SetBool("startFall", true);//first active current Index
        // Debug.Log("MakeAllWayDown");
    }

    public void PushUpFloor()
    {
        _pathAnimator.SetBool("open", false);
        _pathAnimator.SetBool("close", true);
        /*foreach (var item in _paths)
        {
            item.SetBool("startUp", true);//first active current Index
        }*/
        //Debug.Log("PushUpFloor");
    }
}
