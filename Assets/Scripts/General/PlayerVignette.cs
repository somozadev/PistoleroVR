using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace General
{
    public class PlayerVignette : MonoBehaviour
    {
        // [SerializeField] private VolumeProfile volumeProfile;
        // [SerializeField] private VignetteConfig normalConfig;
        // [SerializeField] private VignetteConfig rotationConfig;
        // private Vignette _vignette;
        //
        // public VignetteConfig NormalConfig => normalConfig;
        // public VignetteConfig RotationConfig => rotationConfig;
        //
        // private void OnValidate()
        // {
        //     volumeProfile.TryGet(out _vignette);
        // }
        //
        // public void VignetteTurning(VignetteConfig config)
        // {
        //     _vignette.color = config.color;
        //     _vignette.intensity = config.intensity;
        //     _vignette.smoothness = config.smooth;
        // }
        //
        // [Serializable]
        // public struct VignetteConfig
        // {
        //     public ColorParameter color;
        //     public ClampedFloatParameter intensity;
        //     public ClampedFloatParameter smooth;
        // }
    }
}