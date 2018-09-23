using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour, IEvent
{
    public LevelEventManager.EventType type = LevelEventManager.EventType.Weather;

    ParticleSystem _rainPS;
    ParticleSystem _windPS;
    WeatherEventItem _weather;
    Level _level;
    bool _enabled;
    bool _rainEnabled;
    bool _windEnabled;
    bool _rainCanSpawn;
    bool _windCanSpawn;
    bool _isRaining;
    bool _isWindBlowing;

    float _currRainTimer;
    float _currRainDuration;
    float _currWindTimer;
    float _currWindDuration;

    public void Init(WeatherEventItem item, Level lvl)
    {
        _level = lvl;
        _weather = item;
        _enabled = true;

        _rainCanSpawn = _rainEnabled = _weather.rainAmount != 0;
        _windCanSpawn = _windEnabled = _weather.windAmount != 0;

        _level.MinionManager.OnNewMinionSpawned += NewMinionSpawnedHandler;

        if (_rainEnabled)
        {
            _currRainTimer = Random.Range(_weather.rainCooldown[0], _weather.rainCooldown[1]);
            //_rainPS = GameObject.FindGameObjectWithTag("")
            /*if (_rainPS == null)
                throw new System.Exception("Rain Particle System has not been founded.");*/
        }

        if(_windEnabled)
            _currWindTimer = Random.Range(_weather.windCooldown[0], _weather.windCooldown[1]);

    }

    public void UpdateEvent()
    {
        if (!_enabled) return;

        //this ifs are here because maybe we dont want to have two events at the same time.
        if(!_isWindBlowing)
            ManageRain();

        if (!_isRaining)
            ManageWind();
    }

    void ManageRain()
    {
        if (!_rainEnabled) return;

        if (!_isRaining)
        {
            _currRainTimer -= Time.deltaTime;
            if (_currRainTimer < 0 && _rainCanSpawn)
            {
                BuildRain();
                _weather.rainAmount--;
                if (_weather.rainAmount == 0)
                    _rainCanSpawn = false;
            }
        }

        if (_isRaining)
        {
            _currRainDuration -= Time.deltaTime;
            if (_currRainDuration < 0)
            {
                if (_rainCanSpawn)
                    _currRainTimer = Random.Range(_weather.rainCooldown[0], _weather.rainCooldown[1]);
                else
                    _rainEnabled = false;

                StopRain();
            }
        }
    }

    void BuildRain()
    {
        _isRaining = true;
        _currRainDuration = Random.Range(_weather.rainTime[0], _weather.rainTime[1]);

        if (_rainPS != null)
        {
            var m = _rainPS.main;
            m.loop = false;
            m.duration = _currRainDuration;
            _rainPS.Play();
        }

        _level.LoopThroughMinions(RainDebuff);
    }
    void StopRain()
    {
        _isRaining = false;
        _rainPS.Stop();
        _level.LoopThroughMinions(RainDebuff);
    }
    void RainDebuff(Minion m)
    {
        if (m == null) return;

        if (_isRaining)
            m.DamageDebuff(true, _weather.rainEffectDelta);
        else
            m.DamageDebuff(false);
    }


    void ManageWind()
    {
        if (!_windEnabled) return;

        if (!_isWindBlowing)
        {
            _currWindTimer -= Time.deltaTime;
            if (_currWindTimer < 0 && _windCanSpawn)
            {
                BuildWind();
                _weather.windAmount--;
                if (_weather.windAmount == 0)
                    _windCanSpawn = false;
            }
        }

        if (_isWindBlowing)
        {
            _currWindDuration -= Time.deltaTime;
            if (_currWindDuration < 0)
            {
                if (_windCanSpawn)
                    _currWindTimer = Random.Range(_weather.windCooldown[0], _weather.windCooldown[1]);
                else
                    _windEnabled = false;

                StopWind();
            }
        }
    }

    void BuildWind()
    {
        _isWindBlowing = true;
        _currWindDuration = Random.Range(_weather.windTime[0], _weather.windTime[1]);

        if (_windPS != null)
        {
            var m = _windPS.main;
            m.loop = false;
            m.duration = _currWindDuration;
            _windPS.Play();
        }
        
        _level.LoopThroughMinions(WindDebuff);
    }

    void StopWind()
    {
        _isWindBlowing = false;
        _windPS.Stop();
        _level.LoopThroughMinions(WindDebuff);
    }

    void WindDebuff(Minion m)
    {
        if (m == null) return;

        if (_isWindBlowing)
            m.GetSlowDebuff(0, _weather.windEffectDelta);
        else
            m.StopSlowDebuff();
    }

    #region Handlers
    /// <summary>
    /// If a minion is spawned after an event starts, this will activate the event on those minions
    /// </summary>
    void NewMinionSpawnedHandler()
    {
        if(_isRaining)
            _level.LoopThroughMinions(RainDebuff);

        if(_isWindBlowing)
            _level.LoopThroughMinions(WindDebuff);
    }
    #endregion
}
