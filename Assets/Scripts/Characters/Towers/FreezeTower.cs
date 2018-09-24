using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTower : TowerBase
{
    FreezeTowerStats _freezeTowerStats;
    protected override void Start()
    {
        base.Start();
        _freezeTowerStats = (FreezeTowerStats)towerStats;
    }

    protected override void SpawnProjectile()
    {
        if (pTarget == null) return;

        var random = (SnowBall) projectilePrefabs[Random.Range(0, projectilePrefabs.Count)];
        var p = Instantiate(random, spawnPoint.transform.position, spawnPoint.transform.rotation);
        p.Init(pTarget, towerStats.bulletDamage, towerStats.bulletRange, _freezeTowerStats.freezeTime ,targetType);
    }
}
