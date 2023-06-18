using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    public class LineVisualRendererManager : MonoBehaviour
    {
        [SerializeField] private XRInteractorLineVisual _lineVisual;
        [SerializeField] private GameObject _lineVisualReticlePrefab;
        [SerializeField] private XRRayInteractor _rayInteractor;
        private bool isFullyDisabled = false;

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

        public void DisableIfNotOwner()
        {
            if (_lineVisual.reticle == null)
                _lineVisual.reticle = _lineVisualReticlePrefab;
            _lineVisual.reticle.SetActive(false);
            _lineVisual.enabled = false;
            isFullyDisabled = true;
        }


        private void UnableRay(SelectEnterEventArgs arg)
        {
            if (isFullyDisabled) return;
            if (_lineVisual.reticle == null)
                _lineVisual.reticle = _lineVisualReticlePrefab;
            _lineVisual.reticle.SetActive(false);
            _lineVisual.enabled = false;
        }

        private void EnableRay(SelectExitEventArgs arg)
        {
            if (isFullyDisabled) return;
            if (_lineVisual.reticle == null)
                _lineVisual.reticle = _lineVisualReticlePrefab;
            _lineVisual.reticle.SetActive(true);
            _lineVisual.enabled = true;
        }
    }
}