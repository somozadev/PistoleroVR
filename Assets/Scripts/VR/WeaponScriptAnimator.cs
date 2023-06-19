using System;
using System.Collections;
using UnityEngine;

namespace VR
{
    [RequireComponent(typeof(BaseGun))]
    public class WeaponScriptAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private BaseGun _gun;
        private static readonly int Reload = Animator.StringToHash("Reload");
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int ShootSpeed = Animator.StringToHash("ShootSpeed");
        private static readonly int ReloadSpeed = Animator.StringToHash("ReloadSpeed");

        public void SetReloadSpeed(float speed)
        {
            _animator.SetFloat(ReloadSpeed, speed);
        }

        public void SetShootSpeed(float speed)
        {
            _animator.SetFloat(ShootSpeed, speed);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _gun = GetComponent<BaseGun>();
        }

        public void AnimationShoot()
        {
            _animator.SetTrigger(Shoot);
        }

        public void AnimationReload()
        {
            _animator.SetTrigger(Reload);
        }


        public void EnableShootAgain()
        {
            _gun.canShoot = true;
        }

        public void DisableShootAgain()
        {
            _gun.canShoot = false;
        }

        public string GetCurrentAnimation()
        {
            try
            {
                var returner = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                Debug.Log("NAME IS :  " + returner);
                return returner;
            }
            catch
            {
                return "";
            }
        }
    }
}