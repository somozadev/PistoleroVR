using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace General
{
    
    public class HapticInteractable : MonoBehaviour
    {
        [SerializeField] private Haptic _hapticOnActivated;
        [SerializeField] private Haptic _hapticHoverEntered;
        [SerializeField] private Haptic _hapticHoverExited;
        [SerializeField] private Haptic _hapticSelectEntered;
        [SerializeField] private Haptic _hapticSelectExited;

        private void Start()
        {
            XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
            interactable.activated.AddListener(_hapticOnActivated.TriggerHaptics);
            interactable.hoverEntered.AddListener(_hapticHoverEntered.TriggerHaptics);
            interactable.hoverExited.AddListener(_hapticHoverExited.TriggerHaptics);
            interactable.selectEntered.AddListener(_hapticSelectEntered.TriggerHaptics);
            interactable.selectExited.AddListener(_hapticSelectExited.TriggerHaptics);
        }
    }
}