using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPoolManager<T>
{
    List<T> _list;
	
	public GenericPoolManager(List<T> list)
    {
        _list = list;
    }


}
