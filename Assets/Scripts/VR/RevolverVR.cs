using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using General;
using General.Damageable;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class RevolverVR : MonoBehaviour
    {
        private XRGrabInteractable _interactable;
        [SerializeField] private float _bulletSpeed = 1000.0f;
        [SerializeField] private float _bulletDrop = 0.0f;


        [SerializeField] private ParticleSystem _muzzleParticles;
        [SerializeField] private ParticleSystem _impactParticles;
        [SerializeField] private Transform _raycastOrigin;
        private Ray _ray;
        private RaycastHit _hit;

        [SerializeField] private int currentBullets = 6;
        [SerializeField] private TMP_Text _bulletsText;
        [SerializeField] private GameObject _bulletPrefab;


        private ObjectPooling _bulletsPooling;

        private void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _bulletsText = GetComponentInChildren<TMP_Text>();
            _interactable.activated.AddListener(PerformShoot);
            _bulletsPooling =
                GameManager.Instance.objectPoolingManager.GetNewObjectPool("RevolverVRBullets", ref _bulletPrefab, 5);
        }

        private void Update()
        {
            MoveBulletVR();
            DeleteBullets();
        }

        private void DeleteBullets()
        {
            _bulletsPooling.GetPool().ForEach(
                bullet =>
                {
                    if (!bullet.activeSelf) return;
                    BulletVR bvr = bullet.GetComponent<BulletVR>();
                    if (bvr._time >= bvr._waitTime)
                        bullet.SetActive(false);
                }
            );
        }

        private void PerformShoot(ActivateEventArgs args) => Shoot();

        private Vector3 GetBulletWorldPosition(BulletVR bullet)
        {
            //p + v*t + 0.5*g*t*t
            Vector3 gravity = Vector3.down * _bulletDrop;
            return bullet._initialPos + bullet._initialVel * bullet._time +
                   gravity * (0.5f * bullet._time * bullet._time);
        }

        private void MoveBulletVR()
        {
            _bulletsPooling.GetPool().ForEach(bullet =>
            {
                if (!bullet.activeSelf) return;

                var bulletVR = bullet.GetComponent<BulletVR>();
                Vector3 pos0 = GetBulletWorldPosition(bulletVR);
                bulletVR._time += Time.deltaTime;
                Vector3 pos1 = GetBulletWorldPosition(bulletVR);
                RayCastBulletSegment(pos0, pos1, bulletVR);
            });
        }

        private void RayCastBulletSegment(Vector3 start, Vector3 end, BulletVR bullet)
        {
            Vector3 direction = end - start;
            _ray.origin = start;
            _ray.direction = direction;

            if (Physics.Raycast(_ray, out _hit, direction.magnitude))
            {
                _impactParticles.transform.position = _hit.point;
                _impactParticles.transform.forward = _hit.normal;
                _impactParticles.Emit(1);
                bullet._trail.transform.position = _hit.point;
                bullet._time = 3f;
                CheckForDamage(_hit.transform.gameObject);
            }
            else
            {
                bullet._trail.transform.position = end;
            }
        }

        private void Shoot()
        {
            Vector3 velocity = _raycastOrigin.forward.normalized * _bulletSpeed;

            BulletVR bullet = _bulletsPooling.GetPooledElement().GetComponent<BulletVR>();
            bullet.enabled = true;
            bullet.Init(_raycastOrigin.position, velocity);

            currentBullets--;
            UpdateText();
            _muzzleParticles.Emit(1);
        }

        private void CheckForDamage(GameObject hitObject)
        {
            if (!hitObject.CompareTag("Damageable")) return;
            Damageable damageable = hitObject.GetComponent<Damageable>();
            damageable.Damage(this);
        }

        private void UpdateText() => _bulletsText.text = currentBullets.ToString();

        public void CallShootFromDebugger()
        {
            PerformShoot(null);
        }
    }
}