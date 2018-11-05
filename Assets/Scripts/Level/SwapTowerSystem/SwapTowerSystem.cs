using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SwapTowerSystem : MonoBehaviour
{

    public ParticleSystem swapParticleSysPrefab;

    SwapTowerJson _swapJson;
    /// <summary>
    /// trigger : minion type
    /// consecuence: the tower created to mitigate the use of the trigger.
    /// </summary>
    GenericListJsonLoader<SwapTowerTypeToConsequenceJson> _triggerToConcequence;

    GameManager _gameManager;
    MinionManager _minionManager;
    TowerManager _towerManager;
    List<MinionType> _minionSpawnOrder = new List<MinionType>();
    List<MinionType> _minionWalkFinishedOrder = new List<MinionType>();
    bool _startSystem;
    bool _isEnableForThisLevel;

    void Start()
    {
        LoadSystemInfo();
        _gameManager = GetComponent<GameManager>();
        _gameManager.OnLevelInfoSet += LevelInfoSetHandler;
    }


    void Update()
    {
        if (!_isEnableForThisLevel || !_startSystem) return;

    }


    public void LeveInitFinished(MinionManager minionMan, TowerManager towerMan)
    {
        _minionManager = minionMan;
        _towerManager = towerMan;

        if (!_isEnableForThisLevel) return;

        _startSystem = true;
        _minionManager.OnNewMinionSpawned += NewMinionSpawnHandler;
        _minionManager.OnMinionWalkFinished += MinionWalkFinishedHandler;
    }





    /// <summary>
    /// Do we need the spawn handler?
    /// </summary>
    void NewMinionSpawnHandler(MinionType type)
    {
        _minionSpawnOrder.Add(type);
        //Debug.Log("new minion spawn " + type.ToString());
    }

    void MinionWalkFinishedHandler(MinionType type)
    {
        //Debug.Log("MinionWalkFinishedHandler " + type.ToString());

        _minionWalkFinishedOrder.Add(type);
        if (_minionWalkFinishedOrder.Count < _swapJson.enableThreshold) return;

        var repeatedList = GetRepeatedMinionList(_minionWalkFinishedOrder);
        var minionRep = IsAnyMinionAbuse(repeatedList);
        if (minionRep == null) return;

        var toBeReplacedList = GetTowersTypeToSwap(minionRep);
        var currentTowers = _towerManager.GetLevelTowersByType(toBeReplacedList);
        if (currentTowers.Count == 0) return;
        var selected = currentTowers[UnityEngine.Random.Range(0, currentTowers.Count)];
        var newTower = GetNewTowerPrefab(GameplayUtils.GetTargetTypeByMinionType(minionRep.type));
        SwapTowers(selected, newTower);

        //If the code could arrive to this point, then a swap was made, so reset the list.
        _minionWalkFinishedOrder = new List<MinionType>();
    }

    void SwapTowers(TowerBase toSwap, TowerBase newOne)
    {
        var towerTransform = toSwap.transform;
        var particle = Instantiate<ParticleSystem>(swapParticleSysPrefab, towerTransform.position, Quaternion.identity);
        particle.transform.position = new Vector3(particle.transform.position.x, particle.transform.position.y + 3, particle.transform.position.z);
        particle.transform.position += (Camera.main.transform.position - particle.transform.position).normalized * 2f;
        particle.Play(true);
        Destroy(particle.gameObject, particle.main.duration);
        StartCoroutine(WaitToBuildNewTower(toSwap, newOne, towerTransform));
    }

    IEnumerator WaitToBuildNewTower(TowerBase toSwap, TowerBase newOne, Transform transform)
    {
        yield return new WaitForSeconds(2.1f);
        var newTower = Instantiate<TowerBase>(newOne, transform.position, transform.rotation);
        _towerManager.InitSingleTower(newTower);
        _towerManager.DestroySingleTower(toSwap);
    }

    TowerBase GetNewTowerPrefab(TargetType triggerType)
    {
        var consequence = GetConsequenceByTargetType(triggerType);
        var getRandomNewTowerStringType = consequence.newTowers[UnityEngine.Random.Range(0, consequence.newTowers.Length)];
        TowerType realTowerType = (TowerType)Enum.Parse(typeof(TowerType), getRandomNewTowerStringType);
        foreach (var item in _gameManager.allTowersPrefabs)
        {
            if (item.towerType == realTowerType)
                return item;
        }

        throw new Exception("There isn't a tower of type " + realTowerType + " inside GameManager.allTowersPrefabs, Please add the missing one");
    }


    List<TowerType> GetTowersTypeToSwap(MinionTypeRepetition minionRepeated)
    {
        var targetType = GameplayUtils.GetTargetTypeByMinionType(minionRepeated.type);
        var triggerToConsec = GetConsequenceByTargetType(targetType);


        var list = new List<TowerType>();
        foreach (var item in triggerToConsec.toBeReplaced)
        {
            TowerType t = (TowerType)Enum.Parse(typeof(TowerType), item);
            list.Add(t);
        }
        return list;
    }

    /// <summary>
    /// Is User using to much an specific minion? 
    /// Will return a list of minion, if there are more than one with same high count, return those.
    /// </summary>
    MinionTypeRepetition IsAnyMinionAbuse(List<MinionTypeRepetition> repeatedList)
    {
        var maxRep = 0;
        foreach (var r in repeatedList)
        {
            if (r.count > maxRep)
                maxRep = r.count;
        }

        var filteredList = repeatedList.Where(i => i.count == maxRep).ToList();

        foreach (var item in filteredList)
        {
            var count = 0;//quantity of minions that are ordered.
            for (int i = 0; i < item.orders.Count; i++)
            {
                var currrentOrder = item.orders[i];
                if (item.orders.Count > i + 1)
                {
                    count++;
                    var diff = currrentOrder - item.orders[i + 1];
                    //the order is next to each other ? Add 1, else remove 1
                    count += diff == -1 ? 1 : -1;
                }
            }

            if(count > _swapJson.enableThreshold)
                return item;

            count = 0;
        }
        
        return null;
    }

    List<MinionTypeRepetition> GetRepeatedMinionList(List<MinionType> orderList)
    {
        List<MinionTypeRepetition> repeatedList = new List<MinionTypeRepetition>();
        var order = 0;
        foreach (var minionT in orderList)
        {
            var repeated = repeatedList.FirstOrDefault(r => r.type == minionT);
            if (repeated == null)
            {
                repeated = new MinionTypeRepetition();
                repeated.count++;
                repeated.type = minionT;
                repeatedList.Add(repeated);
            }
            else
            {
                repeated.count++;
            }
            repeated.orders.Add(order);

            order++;
        }

        return repeatedList;
    }

    SwapTowerTypeToConsequenceJson GetConsequenceByTargetType(TargetType t)
    {
        var c = _triggerToConcequence.list.FirstOrDefault(i => (TargetType)Enum.Parse(typeof(TargetType), i.triggerType) == t);
        if(c == null)
        {
            throw new Exception("Modify TriggerToConsequence.json because there is no 'triggerType' named " + t);
        }
        return c;
    }
    void LoadSystemInfo()
    {
        _swapJson = GameUtils.LoadConfig<SwapTowerJson>("SwapTowersSystem.json", GameUtils.CONFIG_PATH + "SwapTowerSystem/");
        _triggerToConcequence = GameUtils.LoadConfig<GenericListJsonLoader<SwapTowerTypeToConsequenceJson>>("TriggerToConsequence.json", GameUtils.CONFIG_PATH + "SwapTowerSystem/");
    }

    void LevelInfoSetHandler(bool gameplayInit)//is this the start of the gameplay or the end?
    {
        if (!gameplayInit)
        {
            Dispose();
            return;
        }

        _isEnableForThisLevel = _swapJson.levelIdExceptions.All(i => i != _gameManager.CurrentLevelInfo.id);
    }

    void Dispose()
    {
        _isEnableForThisLevel = _startSystem = false;
        _minionManager.OnNewMinionSpawned -= NewMinionSpawnHandler;
        _minionManager.OnMinionWalkFinished -= MinionWalkFinishedHandler;

        _minionWalkFinishedOrder = _minionSpawnOrder = new List<MinionType>();
    }
}
