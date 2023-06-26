using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace General
{
    public class InitialFreeGun : MonoBehaviour
    {
        [SerializeField] private XRGrabInteractable _interactable;
        [SerializeField] private GameObject _spotlight;

        private void OnEnable()
        {
            _interactable.selectEntered.AddListener(StartWaveOnGrab);
        }

        private void OnDisable()
        {
            _interactable.selectEntered.RemoveListener(StartWaveOnGrab);
        }

        private void OnDestroy()
        {
            OnDisable();
        }

        private void StartWaveOnGrab(SelectEnterEventArgs arg0)
        {
            GameManager.Instance.players[0].CountdownPlayerCanvas.NewWave();
            Destroy(_spotlight);
            Destroy(this);
        }
    }
}