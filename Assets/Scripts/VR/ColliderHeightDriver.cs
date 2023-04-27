using System;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    public class ColliderHeightDriver : MonoBehaviour
    {
        [Tooltip("The minimum height of the character's collider that will be set by this behavior.")] [SerializeField]
        private float _minHeight;

        [Tooltip("The maximum height of the character's collider that will be set by this behavior.")] [SerializeField]
        private float _maxHeight = float.PositiveInfinity;

        [SerializeField] private LocomotionProvider _locomotionProvider;
        [SerializeField] private XROrigin _xROrigin;
        [SerializeField] private CapsuleCollider _playerCollider;

        private void Awake()
        {
            if (_locomotionProvider == null)
                _locomotionProvider = GetComponent<MovementVR>();
        }

        private void OnEnable()
        {
            Subscribe(_locomotionProvider);
        }

        private void OnDisable()
        {
            Unsubscribe(_locomotionProvider);
        }

        protected void Start()
        {
            SetupCharacterController();
            UpdateCharacterController();
        }

        void SetupCharacterController()
        {
            if (_locomotionProvider == null || _locomotionProvider.system == null)
                return;

            _xROrigin = _locomotionProvider.system.xrOrigin;

            if (_playerCollider == null && _xROrigin != null)
            {
                //error in getting the collider
            }
        }

        private void UpdateCharacterController()
        {
            if (_xROrigin == null || _playerCollider == null)
                return;

            var height = Mathf.Clamp(_xROrigin.CameraInOriginSpaceHeight, _minHeight, _maxHeight);

            Vector3 center = _xROrigin.CameraInOriginSpacePos;
            center.y = height / 2f + _playerCollider.radius;

            _playerCollider.height = height;
            _playerCollider.center = center;
        }

        #region Locomotion

        void Subscribe(LocomotionProvider provider)
        {
            if (provider != null)
            {
                provider.beginLocomotion += OnBeginLocomotion;
                provider.endLocomotion += OnEndLocomotion;
            }
        }

        void Unsubscribe(LocomotionProvider provider)
        {
            if (provider != null)
            {
                provider.beginLocomotion -= OnBeginLocomotion;
                provider.endLocomotion -= OnEndLocomotion;
            }
        }

        void OnBeginLocomotion(LocomotionSystem system)
        {
            UpdateCharacterController();
        }

        void OnEndLocomotion(LocomotionSystem system)
        {
            UpdateCharacterController();
        }

        #endregion
    }
}