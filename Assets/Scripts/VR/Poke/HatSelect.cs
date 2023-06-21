using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR.Poke
{
    public class HatSelect : MonoBehaviour
    {
        [SerializeField] private XRBaseInteractable[] _hats;
        [SerializeField] private TMP_Text[] _hatsUI;
        [SerializeField] private Transform _light;


        [SerializeField] private GameObject _selectedHat;
        
        
        [SerializeField] private AnimationCurve _animationCurveHover = new(
            new Keyframe(0, 0),
            new Keyframe(.5f, .4f),
            new Keyframe(1, 1)
        );

        private void Start()
        {
            foreach (var hat in _hats)
            {
                hat.hoverEntered.AddListener(HoverEnterHat);
                hat.hoverExited.AddListener(HoverExitHat);
                hat.selectEntered.AddListener(SelectHat);
            }
        }


        public void HoverEnterHat(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRRayInteractor)
            {
                var hat = args.interactableObject;
                StartCoroutine(HatHoverEnterAnim(hat));
            }
        }

        public void HoverExitHat(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRRayInteractor)
            {
                var hat = args.interactableObject;
                StartCoroutine(HatHoverExitAnim(hat));
            }
        }

        public void SelectHat(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRRayInteractor)
            {
                var hat = args.interactableObject;
                StartCoroutine(HatSelectedAnim(hat));
                UpdateHatText(hat);
                _selectedHat = hat.transform.gameObject;
            }
        }

        private void UpdateHatText(IXRInteractable hat)
        {
            for (int i = 0; i < _hats.Length; i++)
            {
                if (_hats[i].gameObject == hat.transform.gameObject)
                {
                    string currentText = _hatsUI[i].text;
                }
            }
        }

        private IEnumerator HatHoverEnterAnim(IXRInteractable hat)
        {
            var elapsedTime = 0f;
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                var curvePercent = _animationCurveHover.Evaluate(elapsedTime / 1f);
                hat.transform.localScale =
                    Vector3.Slerp(hat.transform.localScale, new Vector3(120, 120, 120), curvePercent);
                yield return null;
            }
        }

        private IEnumerator HatHoverExitAnim(IXRInteractable hat)
        {
            var elapsedTime = 0f;
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                var curvePercent = _animationCurveHover.Evaluate(elapsedTime / 1f);
                hat.transform.localScale =
                    Vector3.Slerp(hat.transform.localScale, new Vector3(100, 100, 100), curvePercent);
                yield return null;
            }
        }

        private IEnumerator HatSelectedAnim(IXRInteractable hat)
        {
            _light.gameObject.SetActive(true);
            hat.transform.localScale = new Vector3(120, 120, 120);
            var elapsedTime = 0f;
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                var newDirection = Vector3.RotateTowards(_light.forward, (hat.transform.position - _light.position),
                    elapsedTime / 1f, 0f);
                _light.rotation = Quaternion.LookRotation(newDirection);
                yield return null;
            }
        }
    }
}