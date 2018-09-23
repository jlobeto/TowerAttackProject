using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventLoader
{

    WeatherEventList _weatherEventList;


    public LevelEventLoader()
    {
        _weatherEventList = GameUtils.LoadConfig<WeatherEventList>("WeatherEvents.json");
    }

    public WeatherEventItem GetWeatherItem(int levelID)
    {
        foreach (var item in _weatherEventList.weatherEventList)
        {
            if (item.levelId == levelID)
                return item;
        }

        throw new System.Exception("There is not a weather event for level id " + levelID);
    }
}
