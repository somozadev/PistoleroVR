using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    public class LineVisualRendererManager : MonoBehaviour
    {

        [SerializeField] private XRInteractorLineVisual _lineVisual;
        [SerializeField] private XRRayInteractor _rayInteractor;

        private void Awake()
        {
            _rayInteractor = GetComponent<XRRayInteractor>();
            _lineVisual = GetComponent<XRInteractorLineVisual>();
        }

        private void OnEnable()
        {
            _rayInteractor.selectEntered.AddListener(UnableRay);
            _rayInteractor.selectExited.AddListener(EnableRay);
        }

        private void OnDisable()
        {
            _rayInteractor.selectEntered.RemoveListener(UnableRay);
            _rayInteractor.selectExited.RemoveListener(EnableRay);
        }


        private void UnableRay(SelectEnterEventArgs arg)
        {
            _lineVisual.reticle.SetActive(false);
            _lineVisual.enabled = false;
        }

        private void EnableRay(SelectExitEventArgs arg)
        {
            _lineVisual.reticle.SetActive(true);
            _lineVisual.enabled = true;
        }
    }
}