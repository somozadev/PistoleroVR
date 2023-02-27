using General;
using General.Damageable;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit;
using Random = Unity.Mathematics.Random;

namespace VR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class RevolverVR : MonoBehaviour
    {
        private XRGrabInteractable _interactable;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _shootForce;
        [SerializeField] private VisualEffect _shootingParticles;
        [SerializeField] private Transform _bulletPivot;
        [SerializeField] private Vector3 _targetPoint;
        [SerializeField] private float spread;

        [SerializeField] private int currentBullets = 6;
        [SerializeField] private TMP_Text _bulletsText;
        private void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _bulletsText = GetComponentInChildren<TMP_Text>();
            _shootingParticles = GetComponentInChildren<VisualEffect>();
            _interactable.activated.AddListener(Shoot);
            GameManager.Instance.objectPoolingManager.NewObjectPool("RevolverVRBullets", ref _bulletPrefab, 20);
            
        }

        private void Shoot(ActivateEventArgs args)
        {
            currentBullets--;
            UpdateText();
            _shootingParticles.SendEvent("Shoot");

            GameManager.Instance.objectPoolingManager.GetPoolByName("RevolverVRBullets").GetPooledElement(_bulletPivot).GetComponent<BulletVR>().Initialize(_shootForce, transform.forward, _bulletPivot.position);

            RaycastHit hit;
            Ray ray = new Ray(_bulletPivot.position, _bulletPivot.forward);
            Debug.DrawRay(_bulletPivot.position, _bulletPivot.forward, Color.blue,4f);
            if (Physics.Raycast(ray, out hit))
            {
                CheckForDamage(hit.collider.gameObject);
                
                //ammo
                //hp
                
            }
            else
                _targetPoint = ray.GetPoint(60);

            Vector3 directionNoSpread = _targetPoint - _bulletPivot.position;
            float x = UnityEngine.Random.Range(-spread,spread);
            float y = UnityEngine.Random.Range(-spread,spread);

            Vector3 directionSpread = directionNoSpread + new Vector3(x, y, 0);
            
            //instantiate bulletd

        }

        private void CheckForDamage(GameObject hitObject)
        {
            if(!hitObject.CompareTag("Damageable")) return;

            Damageable damageable = hitObject.GetComponent<Damageable>();
            damageable.Damage();

        }

        private void UpdateText() => _bulletsText.text = currentBullets.ToString();
        
        public void CallShootFromDebugger()
        {
            Shoot(null);
        }
    }

}


