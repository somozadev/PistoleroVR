using General;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class RevolverlVR : MonoBehaviour
    {
        private XRGrabInteractable _interactable;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _shootForce;
        [SerializeField] private GameObject _shootingParticles;
        [SerializeField] private Transform _bulletPivot;
        private void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _interactable.activated.AddListener(Shoot);
            GameManager.Instance.objectPoolingManager.NewObjectPool("RevolverVRBullets", ref _bulletPrefab, 20);
            
        }

        private void Shoot(ActivateEventArgs args) => GameManager.Instance.objectPoolingManager.GetPoolByName("RevolverVRBullets").GetPooledElement(_bulletPivot).GetComponent<BulletVR>().Initialize(_shootForce, transform.forward, _bulletPivot.position); 
        

        public void CallShootFromDebugger()
        {
            Shoot(null);
        }
    }

}


