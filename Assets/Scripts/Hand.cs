using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    public float _animationSpeed;
    
    private float _triggerTarget;
    private float _gripTarget;
    private float _triggerCurrent;
    private float _gripCurrent;

    
    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        GetComponentInParent<HandController>().SetHand(this);
    }

    private void Update()
    {
        AnimateHand();
    }

    private void AnimateHand()
    {
        if(_gripCurrent != _gripTarget)
        {
            _gripCurrent = Mathf.MoveTowards(_gripCurrent, _gripTarget, Time.deltaTime * _animationSpeed);
            _animator.SetFloat("Grip", _gripCurrent);
        }
        if(_triggerCurrent != _triggerTarget)
        {
            _triggerCurrent = Mathf.MoveTowards(_triggerCurrent, _triggerTarget, Time.deltaTime * _animationSpeed);
            _animator.SetFloat("Trigger", _triggerCurrent);
        }
    }

    public void SetTrigger(float readValue)
    {
        _triggerTarget = readValue;
    }

    public void SetGrip(float readValue)
    {
        _gripTarget = readValue;
    }
}
