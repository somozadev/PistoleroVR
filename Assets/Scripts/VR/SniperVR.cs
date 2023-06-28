using System;
using General;
using General.Sound;
using UnityEngine;

namespace VR
{
    public class SniperVR : BaseGun
    {
        [SerializeField] private Camera _renderCamera;
        [SerializeField] private RenderTexture _renderTexture;
        [SerializeField] private MeshRenderer _targetScope;

        private void Awake()
        {
            AssignScopeRenderTexture();

            poolingName = "SniperVRBullets";
            maxBullets = 1;
            currentBullets = maxBullets;
            currentTotalBullets = 32;
            maxTotalBullets = currentTotalBullets;
            UpdateText();
        }

        private void AssignScopeRenderTexture()
        {
            _renderTexture = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);
            _renderCamera.targetTexture = _renderTexture;
            _renderTexture.name = _renderCamera.name + "RenderTexture";
            _targetScope.material.mainTexture = _renderTexture;
        }

        protected override void Shoot()
        {
            if (!canShoot) return;
            if (!CheckAmmo()) return;

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

        private void DisableCamera()
        {
            _renderCamera.gameObject.SetActive(false);
        }

        private void EnableCamera()
        {
            _renderCamera.gameObject.SetActive(true);
        }

        protected override void GetGun()
        {
            base.GetGun();
            EnableCamera();
        }

        protected override void DropGun()
        {
            base.DropGun();
            DisableCamera();
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