using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnCustomPointerCallback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    List<Action> _onPointerDownList = new List<Action>();
    List<Action> _onPointerUpList = new List<Action>();

    public enum Listener
    {
        pointerDown,
        pointerUp
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void AddListener(Listener type, Action evt)
    {
        switch (type)
        {
            case Listener.pointerDown:
                _onPointerDownList.Add(evt);
                break;
            case Listener.pointerUp:
                _onPointerUpList.Add(evt);
                break;
        }


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var item in _onPointerDownList)
        {
            item();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (var item in _onPointerUpList)
        {
            item();
        }
    }
}
