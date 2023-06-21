using System;
using General.Sound;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR.Poke
{
    public class ButtonVR : MonoBehaviour
    {
        [SerializeField] private Transform _visualTarget;
        [SerializeField] private Vector3 _localAxis;

        [SerializeField] private float _followAngleTreshold;
        [SerializeField] private float _resetSpeed = 5;
        private Vector3 _initiaLocalPos;
        private bool _freeze;


        private Transform _pokeAttachTrf;
        private Vector3 _offset;

        private XRBaseInteractable _interactable;
        private bool _isFollowing;

        private void Start()
        {
            _initiaLocalPos = _visualTarget.localPosition;
            _interactable = GetComponent<XRBaseInteractable>();
            _interactable.hoverEntered.AddListener(Animate);
            _interactable.hoverExited.AddListener(Reset);
            _interactable.selectEntered.AddListener(Freeze);
        }

        private void Animate(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRPokeInteractor)
            {
                _freeze = false;
                XRPokeInteractor interactor = (XRPokeInteractor)args.interactorObject;
                _isFollowing = true;
                _pokeAttachTrf = interactor.attachTransform;
                _offset = _visualTarget.position - _pokeAttachTrf.position;

                var pokeAngle = Vector3.Angle(_offset, _visualTarget.TransformDirection(_localAxis));
                if (pokeAngle < _followAngleTreshold)
                {
                    _isFollowing = true;
                    _freeze = false;
                }
            }
        }

        private void Reset(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRPokeInteractor)
            {
                _freeze = false;
                _isFollowing = false;
            }
        }

        private void Freeze(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRPokeInteractor)
            {
                _freeze = true;
                AudioManager.Instance.PlayOneShot("UiTap");
            }
        }

        private void Update()
        {
            if (_freeze)
                return;
            if (_isFollowing)
            {
                Vector3 localTargetPos =
                    _visualTarget.InverseTransformPoint(_pokeAttachTrf.position +
                                                        _offset); //get position locally in target
                Vector3 constrainedLocalTargetPos = Vector3.Project(localTargetPos, _localAxis);
                _visualTarget.position = _visualTarget.TransformPoint(constrainedLocalTargetPos);
            }
            else
            {
                _visualTarget.localPosition = Vector3.Lerp(_visualTarget.localPosition, _initiaLocalPos,
                    Time.deltaTime * _resetSpeed);
            }
        }
    }
}