using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace General
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        [SerializeField] private XRInteractionManager _XRInteractionManager;
        [Space(20)] [SerializeField] private XRRayInteractor _leftHandInteractorRay;
        [SerializeField] private XRInteractorLineVisual _leftHandInteractorRayVisual;
        [SerializeField] private LineRenderer _leftHandLineRenderer;
        [SerializeField] private XRRayInteractor _leftHand_HATS_InteractorRay;
        [Space(20)] [SerializeField] private XRRayInteractor _rightHandInteractorRay;
        [SerializeField] private XRInteractorLineVisual _rightHandInteractorRayVisual;
        [SerializeField] private LineRenderer _rightrightHandLineRenderer;
        [SerializeField] private XRRayInteractor _rightHand_HATS_InteractorRay;

        public void DisableInteraction()
        {
            Left(false);
            Right(false);
        }

        public void EnableInteraction()
        {
            Left(true);
            Right(true);
        }

        private void Left(bool action)
        {
            if (!action)
            {
                _leftHandInteractorRay.enabled = false;
                _leftHandInteractorRayVisual.enabled = false;
                _leftHandLineRenderer.enabled = false;
                _leftHand_HATS_InteractorRay.enabled = false;
            }
            else
            {
                _leftHandInteractorRay.enabled = true;
                _leftHandInteractorRayVisual.enabled = true;
                _leftHandLineRenderer.enabled = true;
                _leftHand_HATS_InteractorRay.enabled = true;
            }
        }
        private void Right(bool action)
        {
            if (!action)
            {
                _rightHandInteractorRay.enabled = false;
                _rightHandInteractorRayVisual.enabled = false;
                _rightrightHandLineRenderer.enabled = false;
                _rightHand_HATS_InteractorRay.enabled = false;
            }
            else
            {
                _rightHandInteractorRay.enabled = true;
                _rightHandInteractorRayVisual.enabled = true;
                _rightrightHandLineRenderer.enabled = true;
                _rightHand_HATS_InteractorRay.enabled = true;
            }
        }
    }
}