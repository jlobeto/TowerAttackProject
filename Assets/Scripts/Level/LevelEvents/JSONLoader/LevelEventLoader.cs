using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventLoader
{

    GenericListJsonLoader<WeatherEventItem> _weatherEventList;
    GenericListJsonLoader<EnvironmentEventItem> _environmentEventList;


    public LevelEventLoader()
    {
        _weatherEventList = GameUtils.LoadConfig<GenericListJsonLoader<WeatherEventItem>>("WeatherEvents.json");
        _environmentEventList = GameUtils.LoadConfig<GenericListJsonLoader<EnvironmentEventItem>>("EnvironmentEvents.json");

    }

    public WeatherEventItem GetWeatherItem(int levelID)
    {
        foreach (var item in _weatherEventList.list)
        {
            if (item.levelId == levelID)
                return item;
        }

        throw new System.Exception("There is not a weather event for level id " + levelID);
    }

    public EnvironmentEventItem GetEnvironmentItem(int levelID)
    {
        foreach (var item in _environmentEventList.list)
        {
            if (item.levelId == levelID)
                return item;
        }

        throw new System.Exception("There is not a level event for level id " + levelID);
    }

}
