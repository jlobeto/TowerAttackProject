using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsManager : MonoBehaviour
{
    public Color _baseColor;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform item in transform)
        {
            if(item.tag == "point")
            {
                var mat = item.GetComponent<Renderer>().material;

                /*if (_baseColor == null)
                    _baseColor = mat.GetColor("_EmissionColor");
                    */
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
        
        StartCoroutine(TurnOn());
    }

    IEnumerator TurnOn()
    {
        while(true)
        {
            foreach (Transform item in transform)
            {
                if (item.tag == "point")
                {
                    var mat = item.GetComponent<Renderer>().material;
                    mat.SetColor("_EmissionColor", _baseColor);
                    StartCoroutine(TurnOff(mat));
                    yield return new WaitForSeconds(.05f);
                }
            }
        }
        
    }

    IEnumerator TurnOff(Material item)
    {
        yield return new WaitForSeconds(.5f);
        item.SetColor("_EmissionColor", Color.black);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
