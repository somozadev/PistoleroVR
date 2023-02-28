using System.Collections;
using General.Damageable;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class RevolverVR : MonoBehaviour
    {
        private XRGrabInteractable _interactable;
        [SerializeField] private float _bulletSpeed = 100f;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private VisualEffect _muzzleParticles;
        [SerializeField] private ParticleSystem _impactParticles;
        [SerializeField] private Transform _bulletPivot;
        [SerializeField] private Vector3 _targetPoint;
        [SerializeField] private float spread;
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private LayerMask _raycastLayers;
        [SerializeField] private Vector3 _hitPoint;
        
        [SerializeField] private int currentBullets = 6;
        [SerializeField] private TMP_Text _bulletsText;
        private void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _bulletsText = GetComponentInChildren<TMP_Text>();
            _muzzleParticles = GetComponentInChildren<VisualEffect>();
            _interactable.activated.AddListener(Shoot);
            GameManager.Instance.objectPoolingManager.NewObjectPool("RevolverVRBullets", ref _bulletPrefab, 20);
            
        }

        private void Shoot(ActivateEventArgs args)
        {
            currentBullets--;
            UpdateText();
            _muzzleParticles.SendEvent("Shoot");


            RaycastHit hit;
            Ray ray = new Ray(_bulletPivot.position, _bulletPivot.forward);
            Debug.DrawRay(_bulletPivot.position, _bulletPivot.forward, Color.blue,4f);
            if (Physics.Raycast(ray, out hit, float.MaxValue , layerMask:_raycastLayers))
            {
                _hitPoint = hit.point;
                CheckForDamage(hit.collider.gameObject);

                TrailRenderer tr = Instantiate(_trail, _bulletPivot.position, quaternion.identity);
                StartCoroutine(SpawnTrail(tr, hit));

                // objectpooling for trails and hitpoints

                //ammo
                

            }
            else
                _targetPoint = ray.GetPoint(100);

        }
        private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
        {
            Vector3 startPos = trail.transform.position;
            while (Vector3.Distance(trail.transform.position, hit.transform.position) >= 1f && Vector3.Distance(trail.transform.position, hit.transform.position) <= 100f )
            {
                trail.transform.Translate(trail.transform.forward * Time.deltaTime * _bulletSpeed);
                yield return null;
            }

            trail.transform.position = hit.point;
            Instantiate(_impactParticles, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(trail.gameObject, trail.time);
        }
        private void CheckForDamage(GameObject hitObject)
        {
            if(!hitObject.CompareTag("Damageable")) return;
                
            
            Damageable damageable = hitObject.GetComponent<Damageable>();
            damageable.Damage(this);

        }
        
        public Vector3 GetHitPoint(){return  _hitPoint;}
        private void UpdateText() => _bulletsText.text = currentBullets.ToString();
        
        public void CallShootFromDebugger()
        {
            Shoot(null);
        }
    }

}


