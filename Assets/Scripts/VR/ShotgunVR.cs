using General.Sound;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    public class ShotgunVR : BaseGun
    {
        [SerializeField] private int bulletsSpawned = 4;
        [Range(0f, 0.2f)] [SerializeField] private float spreadAmount;

        private void Awake()
        {
            poolingName = "ShotgunVRBullets";
            maxBullets = 2;
            currentTotalBullets = 48;
            currentBullets = maxBullets;
            maxTotalBullets = currentTotalBullets;
            UpdateText();
        }

        protected override void Shoot()
        {
            if (!canShoot) return;
            if(!CheckAmmo()) return;

            _animator.SetShootSpeed(1.1f);
            _animator.SetReloadSpeed(1.1f);
            currentBullets--;
            base.Shoot();
            for (int i = 0; i < bulletsSpawned; i++)
            {
                Vector3 shootDirection = _raycastOrigin.forward;
                shootDirection += _raycastOrigin.TransformDirection(
                    new Vector3(Random.Range(-spreadAmount, spreadAmount), Random.Range(-spreadAmount, spreadAmount)));

                Vector3 velocity = (shootDirection) * _bulletSpeed;
                BulletVR bullet = _bulletsPooling.GetPooledElement().GetComponent<BulletVR>();
                bullet.enabled = true;
                bullet.Init(_raycastOrigin.position, velocity);
            }


            UpdateText();
            _muzzleParticles.Emit(1);
        }

        protected override void NoShoot()
        {
        }
        protected override void PlaySound()
        {
            AudioManager.Instance.PlayOneShot("Shotgun");
        }
    }
}