using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform _vrTarget;
    public Transform _rigTarget;
    public Vector3 _trackingPosOffset;
    public Vector3 _trackingRotOffset;

    public void Map()
    {
        _rigTarget.position = _vrTarget.TransformPoint(_trackingPosOffset);
        _rigTarget.rotation = _vrTarget.rotation * Quaternion.Euler(_trackingRotOffset);
    }
}
public class RigVR : MonoBehaviour
{
    [SerializeField] private Transform _headConstrain;
    [SerializeField] private Vector3 _headBoddyOffset;
    [SerializeField] private float _turnSmoothness = 5;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    private void Start()
    {
        _headBoddyOffset = transform.position - _headConstrain.position;
    }

    private void LateUpdate()
    {
        transform.position = _headConstrain.position + _headBoddyOffset;
        transform.forward = Vector3.Lerp(transform.forward,
            Vector3.ProjectOnPlane(_headConstrain.forward, Vector3.up).normalized, Time.deltaTime * _turnSmoothness);
        
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
