using General.Sound;
using UnityEngine;

namespace VR
{
    public class RevolverVR : BaseGun
    {
        private void Awake()
        {
            poolingName = "RevolverVRBullets";
            maxBullets = 6;
            currentBullets = maxBullets;
            currentTotalBullets = 36;
            maxTotalBullets = currentTotalBullets;
            UpdateText();
        }

        [ContextMenu("SHOOT")]
        protected override void Shoot()
        {
            if (!canShoot) return;
            if(!CheckAmmo()) return;

            _animator.SetShootSpeed(1f);
            _animator.SetReloadSpeed(1f);
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
            AudioManager.Instance.PlayOneShot("Revolver");
        }
    }
}