using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    const string MINIONS_SAVE_NAME = "minionsSavedData.txt";
    GenericListJsonLoader<MinionBoughtDef> _minionsBoughts;
    public Inventory()
    {
        _minionsBoughts = new GenericListJsonLoader<MinionBoughtDef>();
    }
}
