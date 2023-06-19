using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VR;

namespace General
{
    public class HapticInteractable : MonoBehaviour
    {
        [SerializeField] private Haptic _hapticOnActivated;
        [SerializeField] private Haptic _hapticHoverEntered;
        [SerializeField] private Haptic _hapticHoverExited;
        [SerializeField] private Haptic _hapticSelectEntered;
        [SerializeField] private Haptic _hapticSelectExited;

        [SerializeField] private BaseGun _gun;

        private void Awake()
        {
            _gun = GetComponent<BaseGun>();
        }

        private void Start()
        {
            XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
            interactable.activated.AddListener(HandleActivatedEvent);
            interactable.hoverEntered.AddListener(_hapticHoverEntered.TriggerHaptics);
            interactable.hoverExited.AddListener(_hapticHoverExited.TriggerHaptics);
            interactable.selectEntered.AddListener(_hapticSelectEntered.TriggerHaptics);
            interactable.selectExited.AddListener(_hapticSelectExited.TriggerHaptics);
        }

        private void HandleActivatedEvent(BaseInteractionEventArgs args)
        {
            _hapticOnActivated.TriggerHaptics(args, _gun);
        }
    }
}