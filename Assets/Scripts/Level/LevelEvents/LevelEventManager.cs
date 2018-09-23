using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventManager : MonoBehaviour
{
    public enum EventType
    {
        Weather
    }

    int _levelId;
    List<EventType> _currentEventTypes;
    List<IEvent> _currentEvents = new List<IEvent>();
    GameManager _gameManager;
    Level _lvl;

    bool _eventsEnabled;
    bool _weatherEnabled;

	void Update ()
    {
        if (!_eventsEnabled) return;

        foreach (var item in _currentEvents)
        {
            item.UpdateEvent();
        }
    }

    public void Init(List<EventType> types, int levelId, GameManager gm, Level lvl)
    {
        _levelId = levelId;
        _currentEventTypes = types;
        _gameManager = gm;
        _lvl = lvl;
        var eventsGameObject = new GameObject("EventsManagers");

        if (_currentEventTypes.Contains(EventType.Weather))
        {
            var weatherEvent = _gameManager.LevelEventsLoader.GetWeatherItem(_levelId);
            _weatherEnabled = true;
            var m = eventsGameObject.AddComponent<WeatherManager>();
            m.Init(weatherEvent, _lvl);
            _currentEvents.Add(m);
        }

        if(_weatherEnabled /*o cualquier otro evento*/)
            _eventsEnabled = true;
    }

    public void StopEvents()
    {
        _eventsEnabled = false;
    }
    
}
