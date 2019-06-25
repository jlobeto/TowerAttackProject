using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsManager : MonoBehaviour
{
    List<Material> materials;
    Color _baseColor;

    // Start is called before the first frame update
    void Start()
    {
        //_baseColor = materials[0].GetColor("_EmissionColor");

        foreach (Transform item in transform)
        {
            if(item.tag == "point")
            {
                item.GetComponent<Material>().
            }
        }
        /*
        foreach (var item in materials)
        {
            item.SetColor("_EmissionColor", Color.black);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
