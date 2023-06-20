using General.Sound;
using UnityEngine;

namespace VR
{
    public class SniperVR : BaseGun
    {
        private void Awake()
        {
            poolingName = "SniperVRBullets";
            maxBullets = 1;
            currentBullets = maxBullets;
            currentTotalBullets = 32;
            maxTotalBullets = currentTotalBullets;
            UpdateText();
        }

        protected override void Shoot()
        {
            if (!canShoot) return;
            if(!CheckAmmo()) return;

            _animator.SetShootSpeed(0.8f);
            _animator.SetReloadSpeed(0.8f);
            currentBullets--;
            base.Shoot();
            Vector3 velocity = _raycastOrigin.forward.normalized * _bulletSpeed;

            BulletVR bullet = _bulletsPooling.GetPooledElement().GetComponent<BulletVR>();
            bullet.enabled = true;
            bullet.Init(_raycastOrigin.position, velocity);

            UpdateText();
            _muzzleParticles.Emit(1);
        }

        protected override void NoShoot()
        {
        }

        protected override void PlaySound()
        {
            AudioManager.Instance.PlayOneShot("Sniper");
        }
    }
}