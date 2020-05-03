using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager
{
    GameManager _gameManager;
    Dictionary<LibraryCategory, GenericListJsonLoader<LibraryCategoryTypeInfoDef>> _dict;

    public LibraryManager(GameManager gm)
    {
        _dict = new Dictionary<LibraryCategory, GenericListJsonLoader<LibraryCategoryTypeInfoDef>>();
        _gameManager = gm;
        var data = GameUtils.LoadConfig<GenericListJsonLoader<LibraryCategoryTypeInfoDef>>("LibraryMinionsConfig.json", GameUtils.LIBRARY_CONFIG_PATH);
        _dict.Add(LibraryCategory.Minions, data);

        data = GameUtils.LoadConfig<GenericListJsonLoader<LibraryCategoryTypeInfoDef>>("LibraryTowersConfig.json", GameUtils.LIBRARY_CONFIG_PATH);
        _dict.Add(LibraryCategory.Towers, data);
    }


    public LibraryCategoryTypeInfoDef GetLibraryData(LibraryCategory cat, string type)
    {
        foreach (var item in _dict[cat].list)
        {
            if (item.type == type)
                return item;
        }

        return new LibraryCategoryTypeInfoDef();
    }
}
