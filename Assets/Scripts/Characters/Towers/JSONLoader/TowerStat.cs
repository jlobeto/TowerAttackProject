using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TowerStat 
{
	//all
	public int levelId = 1;
	public int towerLevel = 1;
	public float fireCooldown = 1f;
	public float fireRange = 8f;
	public float bulletDamage = 5;
	public float bulletRange = 0;
	public float rotationSpeed = 5f;

	//ice tower
	public float deltaSpeed = 0.8f;

	//freeze tower
	public float freezeTime = 1f;
}
