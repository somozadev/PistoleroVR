using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class BulletVR : MonoBehaviour
{
    public float _time;
    public Vector3 _initialPos;
    public TrailRenderer _trail;
    public Vector3 _initialVel;
    public float _waitTime;

    private void OnEnable()
    {
        _trail.Clear();
    }

    private void OnDisable()
    {
        _trail.Clear();
    }

    private void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
    }

    public void Init(Vector3 initialPos, Vector3 initialVel)
    {
        _initialPos = initialPos;
        _initialVel = initialVel;
        _time = 0.0f;
        _waitTime = 3.0f;
        _trail.AddPosition(initialPos);
    }
}