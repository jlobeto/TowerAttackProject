using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour {


    public void DissolveStopped()
    {
        var m = GetComponentInParent<Minion>();
        if (m != null)
        {
            m.DissolveStopped();
        }
    }
}
