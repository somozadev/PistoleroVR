using System;
using System.Collections;
using UnityEngine;

namespace General.Damageable
{
    public class BullsEye : Damageable
    {
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Damage()
        {
            _animator.Play("Hit");
            StartCoroutine(SlowReset());
        }

        private IEnumerator SlowReset()
        {
            yield return new WaitForSeconds(1f);
            _animator.Play("Restore");
        }
    }
}