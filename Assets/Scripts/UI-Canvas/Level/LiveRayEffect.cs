using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiveRayEffect : MonoBehaviour
{
    public float timeMoving = 1f;
    bool _active;
    float _currTime;
    Vector3 _from, _to;
    Image _img;
    Color _endColor = new Color(0, .8f, .2f);

    void Start ()
    {
        _img = GetComponent<Image>();
    }
	

	void Update ()
    {
        if (!_active) return;

        _currTime += Time.deltaTime / timeMoving;
        transform.position = Vector3.Lerp(_from, _to, _currTime);

        if(_currTime < 0.5f)
        {
            _img.color = Color.Lerp(Color.black, Color.white, _currTime * 2);
        }
        else if(_currTime > 0.8f)
        {
            _img.color = Color.Lerp(Color.white, _endColor, _currTime);
        }
        
        if (Mathf.Abs(Vector3.Distance(transform.position, _to)) < 0.3f)
        {
            transform.position = new Vector3(1000, 1000, 1000);
            _active = false;
        }
    }

    /// <summary>
    /// Use the screen position FROM and TO to move.
    /// </summary>
    public void Init(Vector3 from , Vector3 to)
    {
        _active = true;
        _from = from;
        _to = to;
    }
}
