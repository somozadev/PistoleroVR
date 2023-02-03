using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{

    [SerializeField] private ActionBasedController _controller;
    [SerializeField] private Hand _hand;
    
    
    
    
    void Awake()
    {
        _controller = GetComponent<ActionBasedController>();
    }
    
    public void SetHand(Hand hand){ _hand = hand;}
    private void Update()
    {
        if(_hand==null)
            return;
        _hand.SetGrip(_controller.selectAction.action.ReadValue<float>());
        _hand.SetTrigger(_controller.activateAction.action.ReadValue<float>());
    }
   
}
