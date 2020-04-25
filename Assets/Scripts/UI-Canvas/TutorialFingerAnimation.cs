using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFingerAnimation : MonoBehaviour
{

    bool _isAnimating;
    Transform _finalPos;
    Vector3 _posInit;
    Quaternion _rotInit;


    void Start()
    {
        
    }

    
    void Update()
    {
        if (!_isAnimating) return;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, _finalPos.localPosition, Time.deltaTime * 390);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _finalPos.localRotation, Time.deltaTime * 2);

        if(Vector3.Distance(transform.localPosition, _finalPos.localPosition) <= 10f)
        {
            transform.localPosition = _posInit;
            transform.localRotation = _rotInit;
        }
    }

    public void InitAnimation(Transform initPos, Transform finalPos)
    {
        _isAnimating = true;
        _finalPos = finalPos;
        _posInit = initPos.localPosition;
        _rotInit = initPos.localRotation;
    }

    public void StopAnimation()
    {
        _isAnimating = false;
        GetComponent<Image>().enabled = false;
    }
}
