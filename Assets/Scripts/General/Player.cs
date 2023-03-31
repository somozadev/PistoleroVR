using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR.Interaction.Toolkit;

namespace General
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private MovementVR _movementVR;
        [SerializeField] private RigVR _rigVR;
        [SerializeField] private CharacterCustomization _characterCustomization;
        [SerializeField] private XRInteractorLineVisual _leftHand;
        [SerializeField] private XRInteractorLineVisual _rightHand;

        private void Awake()
        {
            _movementVR = GetComponentInChildren<MovementVR>();
            _rigVR = GetComponentInChildren<RigVR>();
            _characterCustomization = GetComponent<CharacterCustomization>();
            _leftHand = GetComponentsInChildren<XRInteractorLineVisual>()[1];
            _rightHand = GetComponentsInChildren<XRInteractorLineVisual>()[0];
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.player = this;
            }
        }

    }
}