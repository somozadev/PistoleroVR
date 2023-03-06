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
        [SerializeField] private ParticleSystem _muzzleParticles;
        [SerializeField] private ParticleSystem _impactParticles;
        [SerializeField] private TrailRenderer _bulletTrail;
        [SerializeField] private Transform _raycastOrigin;
        private Ray _ray;
        private RaycastHit _hit;


        [SerializeField] private int currentBullets = 6;
        [SerializeField] private TMP_Text _bulletsText;

        private void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _bulletsText = GetComponentInChildren<TMP_Text>();
            _interactable.activated.AddListener(Shoot);


            // GameManager.Instance.objectPoolingManager.NewObjectPool("RevolverVRBullets", ref _bulletPrefab, 20);
        }

        private void Shoot(ActivateEventArgs args)
        {
            currentBullets--;
            UpdateText();
            _muzzleParticles.Emit(1);


            _ray.origin = _raycastOrigin.position;
            _ray.direction = _raycastOrigin.forward;

            var tracer = Instantiate(_bulletTrail, _ray.origin, quaternion.identity);
            tracer.AddPosition(_ray.origin);
            
            if (Physics.Raycast(_ray, out _hit))
            {
                _impactParticles.transform.position = _hit.point;
                _impactParticles.transform.forward = _hit.normal;
                _impactParticles.Emit(1);
                tracer.transform.position = _hit.point;
                //hitted an object! 
            }
            else
            {
                tracer.transform.position = _ray.GetPoint(10);
                
            }


            //
            // RaycastHit hit;
            // Ray ray = new Ray(_bulletPivot.position, _bulletPivot.forward);
            // Debug.DrawRay(_bulletPivot.position, _bulletPivot.forward, Color.blue,4f);
            // if (Physics.Raycast(ray, out hit, float.MaxValue , layerMask:_raycastLayers))
            // {
            //     _hitPoint = hit.point;
            //     CheckForDamage(hit.collider.gameObject);
            //
            //     TrailRenderer tr = Instantiate(_trail, _bulletPivot.position, quaternion.identity);
            //     StartCoroutine(SpawnTrail(tr, hit));
            //
            //     // objectpooling for trails and hitpoints
            //
            //     //ammo
            //     
            //
            // }
            // else
            //     _targetPoint = ray.GetPoint(100);
        }

        // private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
        // {
        //     Vector3 startPos = trail.transform.position;
        //     while (Vector3.Distance(trail.transform.position, hit.transform.position) >= 1f && Vector3.Distance(trail.transform.position, hit.transform.position) <= 100f )
        //     {
        //         trail.transform.Translate(trail.transform.forward * Time.deltaTime * _bulletSpeed);
        //         yield return null;
        //     }
        //
        //     trail.transform.position = hit.point;
        //     Instantiate(_impactParticles, hit.point, Quaternion.LookRotation(hit.normal));
        //     Destroy(trail.gameObject, trail.time);
        // }
        private void CheckForDamage(GameObject hitObject)
        {
            if (!hitObject.CompareTag("Damageable")) return;


            Damageable damageable = hitObject.GetComponent<Damageable>();
            damageable.Damage(this);
        }

        // public Vector3 GetHitPoint(){return  _hitPoint;}
        private void UpdateText() => _bulletsText.text = currentBullets.ToString();

        public void CallShootFromDebugger()
        {
            Shoot(null);
        }
    }
}