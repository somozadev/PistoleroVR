using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    public class RevolverVR : BaseGun
    {
        private void Awake()
        {
            poolingName = "RevolverVRBullets";
        }

        protected override void Shoot()
        {
            Vector3 velocity = _raycastOrigin.forward.normalized * _bulletSpeed;

            BulletVR bullet = _bulletsPooling.GetPooledElement().GetComponent<BulletVR>();
            bullet.enabled = true;
            bullet.Init(_raycastOrigin.position, velocity);

            currentBullets--;
            UpdateText();
            _muzzleParticles.Emit(1);
        }

        protected override void NoShoot()
        {
        }
    }
}