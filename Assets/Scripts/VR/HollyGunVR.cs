using UnityEngine;
using VR;

public class HollyGunVR : BaseGun
{
    [SerializeField] private Transform _raycastOrigin2;
    [SerializeField] private Transform _raycastOrigin3;
    [SerializeField] private Transform _raycastOrigin4;

    [SerializeField] private ParticleSystem _muzzleParticles2;
    [SerializeField] private ParticleSystem _muzzleParticles3;
    [SerializeField] private ParticleSystem _muzzleParticles4;

    private void Awake()
    {
        poolingName = "HollyGunVRBullets";
        maxBullets = 3;
        currentBullets = maxBullets;
        currentTotalBullets = 30;
        maxTotalBullets = currentTotalBullets;
        UpdateText();
    }

    private void ShootFromGivenOrigin(Transform origin, ParticleSystem particles)
    {
        Vector3 velocity = origin.forward.normalized * _bulletSpeed;
        BulletVR bullet = _bulletsPooling.GetPooledElement().GetComponent<BulletVR>();
        bullet.enabled = true;
        bullet.Init(origin.position, velocity);
        particles.Emit(1);
    }

    protected override void Shoot()
    {
        if (!canShoot) return;

        _animator.SetShootSpeed(1f);
        _animator.SetReloadSpeed(0.8f);
        base.Shoot();
        ShootFromGivenOrigin(_raycastOrigin, _muzzleParticles);
        ShootFromGivenOrigin(_raycastOrigin2, _muzzleParticles2);
        ShootFromGivenOrigin(_raycastOrigin3, _muzzleParticles3);
        ShootFromGivenOrigin(_raycastOrigin4, _muzzleParticles4);
        currentBullets--;
        UpdateText();
    }

    protected override void NoShoot()
    {
    }
}