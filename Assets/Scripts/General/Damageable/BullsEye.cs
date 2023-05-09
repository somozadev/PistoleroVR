using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using VR;

namespace General.Damageable
{
    public class BullsEye : Damageable
    {
        [SerializeField] private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _health = 1;
        }

        public override void Damage(RevolverVR revolverVR)
        {
            _health -= 1;

            if (_health <= 0)
            {
                _animator.SetTrigger("Hit");
                StartCoroutine(SlowReset());
            }
        }

        

        private IEnumerator SlowReset()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetTrigger("Restore");
            _health = 1;
        }
    }

}