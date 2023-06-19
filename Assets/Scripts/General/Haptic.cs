using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VR;

namespace General
{
    [System.Serializable]
    public class Haptic
    {
        [SerializeField] [Range(0, 1)] private float _intensity;
        [SerializeField] private float _duration;

        public Haptic(float intensity, float duration)
        {
            _intensity = intensity;
            _duration = duration;
        }

        public void TriggerHaptics(BaseInteractionEventArgs eventArgs)
        {
            if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
            {
                TriggerHaptics(controllerInteractor.xrController);
            }
        }

        public void TriggerHaptics(BaseInteractionEventArgs eventArgs , BaseGun gun)
        {
            if(!gun.canShoot) return;
            if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
            {
                TriggerHaptics(controllerInteractor.xrController);
            }
        }

        private void TriggerHaptics(XRBaseController controller)
        {
            if (_intensity > 0)
                controller.SendHapticImpulse(_intensity, _duration);
        }
    }
}