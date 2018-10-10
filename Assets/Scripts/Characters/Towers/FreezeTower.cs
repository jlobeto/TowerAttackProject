using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTower : TowerBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void SpawnProjectile()
    {
        if (pTarget == null) return;

        var random = (SnowBall) projectilePrefabs[Random.Range(0, projectilePrefabs.Count)];
        var p = Instantiate(random, spawnPoint.transform.position, spawnPoint.transform.rotation);
		p.Init(pTarget, pMyStat.bulletDamage, pMyStat.bulletRange, pMyStat.freezeTime ,targetType);
    }
}
