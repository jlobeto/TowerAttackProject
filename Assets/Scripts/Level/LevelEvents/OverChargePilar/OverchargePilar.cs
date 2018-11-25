using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargePilar : MonoBehaviour
{
    public List<TowerBase> affected;
    public Transform trailSpawnPoint;
    public LineRenderer line;

    Color activatedLineColor = new Color(0, 200f / 255f, 255);
    Color deactivatedLineColor;

    List<LineRenderer> _linesToTower;
    bool _isActive;
    float _effectTimeAux;
	
	void Start ()
    {
        _linesToTower = new List<LineRenderer>();
        line.SetPosition(0, trailSpawnPoint.position);
        line.SetPosition(1, affected[0].transform.position);
        deactivatedLineColor = line.startColor;

        for (int i = 1; i < affected.Count; i++)
        {
            var l = Instantiate<LineRenderer>(line, trailSpawnPoint);
            line.SetPosition(0, trailSpawnPoint.position);
            line.SetPosition(1, affected[i].transform.position);
            _linesToTower.Add(l);
        }
    }
	
	
	void Update ()
    {

    }

    /// <summary>
    /// Will be a clickable pilar. Manage the effects here.
    /// </summary>
    public void ActivatePilar(float effectTime)
    {
        _effectTimeAux = effectTime;
        _isActive = true;

    }

    public void DeactivatePilar()
    {
        _isActive = false;
    }

    public void StunTowers()
    {

        foreach (var item in affected)
        {
            item.ReceiveStun(_effectTimeAux);
        }

        foreach (var item in _linesToTower)
        {
            item.startColor = activatedLineColor;
            item.endColor = activatedLineColor;
        }
    }

    IEnumerator WaitUntilEffectTimeEnd()
    {
        yield return new WaitForSeconds(_effectTimeAux);

        foreach (var item in _linesToTower)
        {
            item.startColor = deactivatedLineColor;
            item.endColor = deactivatedLineColor;
        }
    }
}
