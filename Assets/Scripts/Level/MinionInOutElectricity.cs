using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionInOutElectricity : ElectroshockElectricity
{
    public float electricityDuration = 0.5f;

    bool _isIn;
    WalkNode _targetNode;


    /*protected override void Start()
    {
        pointsCount = 2;
        half = 1;

        _lRend = GetComponent<LineRenderer>();
        points = new Vector3[pointsCount];
        _lRend.positionCount = pointsCount;
    }*/

    protected override void Update()
    {
        if (!_enable) return;

        base.Update();
        
    }

    /// <summary>
    /// This func has to be executable within level initialization
    /// isIn = is for when spawning a new minion? true
    /// </summary>
    public void Init(WalkNode targetNode)
    {
        _targetNode = targetNode;
        
    }

    /*protected override void CalculatePoints()
    {
        timer += Time.deltaTime;

        if (timer > timerTimeOut)
        {
            timer = 0;

            points[pointIndexA] = transformPointA.position;
            points[pointIndexB] = transformPointB.position;
            //points[pointIndexB] = GetCenter(points[pointIndexA], points[pointIndexC]);

            float distance = Vector3.Distance(transformPointA.position, transformPointB.position) / points.Length;
            mainTextureScale.x = distance;
            mainTextureOffset.x = Random.Range(-randomness, randomness);
            //_lRend.sharedMaterial.SetTextureScale(mainTexture, mainTextureScale);
            _lRend.sharedMaterial.SetTextureOffset(mainTexture, mainTextureOffset);

            randomness = distance / (pointsCount * half);

            SetRandomness();

            _lRend.SetPositions(points);
        }
    }
    */
    public void InitElectricity()
    {
        _enable = true;
        _lRend.enabled = true;
        StartCoroutine(WaitElectricityDuration());
    }

    /*protected override void SetRandomness()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i != pointIndexA && i != pointIndexC)
            {
                points[i].x += Random.Range(-randomness, randomness);
                points[i].y += Random.Range(-randomness, randomness);
                points[i].z += Random.Range(-randomness, randomness);
            }
        }
    }*/

    IEnumerator WaitElectricityDuration()
    {
        yield return new WaitForSeconds(electricityDuration);
        DisableElectricity();
    }
}
