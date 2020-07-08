using UnityEngine;
using System.Collections;

public class ElectroshockElectricity : MonoBehaviour
{
    public Transform transformPointA;
    public Transform transformPointB;

    protected LineRenderer _lRend;
    protected bool _enable;

    protected int pointsCount = 5;
    protected int half = 2;
    protected float randomness;
    protected Vector3[] points;

    protected readonly int pointIndexA = 0;
    protected readonly int pointIndexB = 1;
    protected readonly int pointIndexC = 2;
    protected readonly int pointIndexD = 3;
    protected readonly int pointIndexE = 4;

    protected readonly string mainTexture = "_MainTex";
    protected Vector2 mainTextureScale = Vector2.one;
    protected Vector2 mainTextureOffset = Vector2.one;

    protected float timer;
    protected float timerTimeOut = 0.05f;

    

    protected virtual void Start ()
    {
        _lRend = GetComponent<LineRenderer>();
        points = new Vector3[pointsCount];
        _lRend.positionCount = pointsCount;
    }

    protected virtual void Update()
    {
        if(_enable)
            CalculatePoints();
    }

    public void DisableElectricity()
    {
        _enable = false;
        _lRend.enabled = false;
    }

    public void EnableElectricity()
    {
        _enable = true;
        _lRend.enabled = true;
    }

    protected virtual void CalculatePoints()
    {
        timer += Time.deltaTime;

        if (timer > timerTimeOut)
        {
            timer = 0;

            points[pointIndexA] = transformPointA.position;
            points[pointIndexE] = transformPointB.position;
            points[pointIndexC] = GetCenter(points[pointIndexA], points[pointIndexE]);
            points[pointIndexB] = GetCenter(points[pointIndexA], points[pointIndexC]);
            points[pointIndexD] = GetCenter(points[pointIndexC], points[pointIndexE]);

            float distance = Vector3.Distance(transformPointA.position, transformPointB.position) / points.Length;
            mainTextureScale.x = distance;
            mainTextureOffset.x = Random.Range(-randomness, randomness);
            _lRend.sharedMaterial.SetTextureScale(mainTexture, mainTextureScale);
            _lRend.sharedMaterial.SetTextureOffset(mainTexture, mainTextureOffset);

            randomness = distance / (pointsCount * half);

            SetRandomness();

            _lRend.SetPositions(points);
        }
    }

    protected virtual void SetRandomness()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i != pointIndexA && i != pointIndexE)
            {
                points[i].x += Random.Range(-randomness, randomness);
                points[i].y += Random.Range(-randomness, randomness);
                points[i].z += Random.Range(-randomness, randomness);
            }
        }
    }

    protected Vector3 GetCenter(Vector3 a, Vector3 b)
    {
        return (a + b) / half;
    }
}
