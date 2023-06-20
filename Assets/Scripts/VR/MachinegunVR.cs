using System.Collections;
using System.Linq;
using General;
using General.Sound;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    public class MachinegunVR : BaseGun
    {
        [SerializeField] private float fireRate = 30;
        [Range(0f, 0.2f)] [SerializeField] private float spreadAmount;
        private bool isShooting;

        private void Awake()
        {
            poolingName = "MachinegunVRBullets";
            maxBullets = 65;
            currentBullets = maxBullets;
            currentTotalBullets = 325;
            maxTotalBullets = currentTotalBullets;
            UpdateText();
        }

        protected override void Shoot()
        {
            if (!canShoot) return;
            if (!CheckAmmo()) return;

            _animator.SetShootSpeed(1.7f);
            _animator.SetReloadSpeed(0.5f);
            isShooting = true;
            StartCoroutine(ShootingLoop());
        }

        protected override void NoShoot()
        {
            isShooting = false;
            StopCoroutine(ShootingLoop());
        }

        protected override void DropGun()
        {
            base.DropGun();
            NoShoot();
        }

        private IEnumerator ShootingLoop()
        {
            while (isShooting)
            {
                if (!CheckAmmo()) break;
                if (_animator.GetCurrentAnimation() == "Reload") break;
                currentBullets--;
                base.Shoot();
                GameManager.Instance.players.First().leftController.SendHapticImpulse(.5f, fireRate / 200);
                GameManager.Instance.players.First().rightController.SendHapticImpulse(.5f, fireRate / 200);
                Vector3 shootDirection = _raycastOrigin.forward;
                shootDirection += _raycastOrigin.TransformDirection(
                    new Vector3(Random.Range(-spreadAmount, spreadAmount), Random.Range(-spreadAmount, spreadAmount)));

                Vector3 velocity = (shootDirection) * _bulletSpeed;

                BulletVR bullet = _bulletsPooling.GetPooledElement().GetComponent<BulletVR>();
                bullet.enabled = true;
                bullet.Init(_raycastOrigin.position, velocity);

                UpdateText();
                _muzzleParticles.Emit(1);
                yield return new WaitForSeconds(fireRate / 100);
            }
        }

        protected override void PlaySound()
        {
            AudioManager.Instance.PlayOneShot("Machinegun");
        }
    }
}