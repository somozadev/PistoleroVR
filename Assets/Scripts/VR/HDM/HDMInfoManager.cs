using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;


public class HDMInfoManager : MonoBehaviour
{
    [SerializeField] private bool _devieActive;
    [SerializeField] private string _devieName;
    public XRDeviceSimulator _deviceSimulator;    
    private void Start()
    {
        _devieActive = XRSettings.isDeviceActive;
        _devieName = XRSettings.loadedDeviceName;
        _deviceSimulator.enabled = false ;

        if (!_devieActive)
            Debug.Log("No headset plugged");
        
        else if (XRSettings.isDeviceActive && (_devieName == "Mock HMD") || (_devieName == "MockHMD Display"))
        {
            Debug.Log("Using mock HMD");
            _deviceSimulator.enabled = true;
        }
        else
            Debug.Log("Headset used " + _devieName);
        
        
    }
}