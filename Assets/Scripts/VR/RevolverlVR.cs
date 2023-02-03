using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class RevolverlVR : MonoBehaviour
    {
        private XRGrabInteractable _interactable;
        [SerializeField] private BulletVR _bulletPrefab;
        [SerializeField] private float _shootForce;
        [SerializeField] private GameObject _shootingParticles;
        [SerializeField] private Transform _bulletPivot;

        private void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _interactable.activated.AddListener(Shoot);
        }

        private void Shoot(ActivateEventArgs args)
        {
            BulletVR bulletInstance = Instantiate(_bulletPrefab,_bulletPivot.position, quaternion.identity);
            bulletInstance.Initialize(_shootForce, transform.forward);
        }
    }

}