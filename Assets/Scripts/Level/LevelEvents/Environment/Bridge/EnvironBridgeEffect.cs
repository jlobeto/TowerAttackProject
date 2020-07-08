using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironBridgeEffect : MonoBehaviour
{
    public string destination = "";

    Animator _pathAnimator;
    float _timerToPlayNext = 0.3f;
    float _timerToPlayNextAux;
    bool _activate;
    int _pathIndexToAnimate;

    void Start()
    {
        _pathAnimator = GetComponentInChildren<Animator>();
        //Debug.Log(destination + " ... paths count " + _paths.Count);
        _timerToPlayNextAux = _timerToPlayNext;
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
