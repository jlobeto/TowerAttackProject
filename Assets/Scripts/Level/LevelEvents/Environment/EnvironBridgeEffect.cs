using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironBridgeEffect : MonoBehaviour
{
    public string destination = "";

    List<Animator> _paths = new List<Animator>();
    float _timerToPlayNext = 0.2f;

    void Start()
    {
        _paths = GetComponentsInChildren<Animator>().ToList();
    }
    

    void Update()
    {

    }

    public void ActivateAnimation(bool forward)
    {

        foreach (var item in _paths)
        {
            item.SetFloat("animSpeed", forward ? 1 : -1);
            StartCoroutine(MakeFall(item));
        }
    }

    IEnumerator MakeFall(Animator item)
    {
        yield return new WaitForSeconds(_timerToPlayNext);
        item.SetBool("startFall", true);
    }
}
