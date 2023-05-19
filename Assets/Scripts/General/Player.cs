using System;
using System.Diagnostics;
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
        [SerializeField] private PlayerData _playerData;

        public XRBaseController leftController;
        public XRBaseController rightController;
        
        public PlayerData PlayerData => _playerData;
        public MovementVR PlayerMovement => _movementVR;
        private void Awake()
        {
            _playerData = GetComponent<PlayerData>();
            _movementVR = GetComponentInChildren<MovementVR>();
            _rigVR = GetComponentInChildren<RigVR>();
            _characterCustomization = GetComponent<CharacterCustomization>();
            _leftHand = GetComponentsInChildren<XRInteractorLineVisual>()[1];
            _rightHand = GetComponentsInChildren<XRInteractorLineVisual>()[0];
        }

        private void OnValidate()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.players.Add(this);
            }
        }

        private void Start()
        {
            _movementVR.transform.localPosition = Vector3.zero;
        }
    }
}