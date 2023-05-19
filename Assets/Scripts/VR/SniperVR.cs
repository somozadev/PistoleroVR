using UnityEngine;

namespace VR
{
    public class SniperVR : BaseGun
    {
        private void Awake()
        {
            poolingName = "SniperVRBullets";
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