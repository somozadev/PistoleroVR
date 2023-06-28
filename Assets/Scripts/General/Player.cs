using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR.Interaction.Toolkit;

namespace General
{
    public class Player : MonoBehaviour
    {
        
        [SerializeField] private MovementVR _movementVR;
        [SerializeField] private RigVR _rigVR;
        [SerializeField] private PlayerInteractionManager _playerInteractionManager;
        [SerializeField] private CharacterCustomization _characterCustomization;
        [SerializeField] private XRInteractorLineVisual _leftHand;
        [SerializeField] private XRInteractorLineVisual _rightHand;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private CountdownPlayerCanvas _countdownPlayerCanvas;
        [SerializeField] private PlayerIngameCanvas _playerIngameCanvas;
        [SerializeField] private GameOverCanvas _playerGameOverCanvas;
        public XRBaseController leftController;
        public XRBaseController rightController;

        public PlayerData PlayerData => _playerData;
        public MovementVR PlayerMovement => _movementVR;
        public PlayerHealth PlayerHealth => _playerHealth;

        public CountdownPlayerCanvas CountdownPlayerCanvas => _countdownPlayerCanvas;
        public PlayerIngameCanvas PlayerIngameCanvas => _playerIngameCanvas;
        public GameOverCanvas PlayerGameOverCanvas => _playerGameOverCanvas;
        public PlayerInteractionManager PlayerInteractionManager => _playerInteractionManager;

        [Header("current item in hands")] [SerializeField]
        public GameObject leftHandItem;

        [SerializeField] public GameObject rightHandItem;

        private void Awake()
        {
            _playerInteractionManager = GetComponent<PlayerInteractionManager>();
            _playerHealth = GetComponentInChildren<PlayerHealth>();
            _playerData = GetComponent<PlayerData>();
            _movementVR = GetComponentInChildren<MovementVR>();
            _rigVR = GetComponentInChildren<RigVR>();
            _characterCustomization = GetComponent<CharacterCustomization>();
            _leftHand = GetComponentsInChildren<XRInteractorLineVisual>()[1];
            _rightHand = GetComponentsInChildren<XRInteractorLineVisual>()[0];
            _countdownPlayerCanvas = GetComponent<CountdownPlayerCanvas>();
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