using System;
using System.Collections;
using UnityEngine;

namespace General.Damageable
{
    public class BullsEye : Damageable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _centerPivot;
        [SerializeField] private Transform _radiusPivot;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Damage()
        {
            Debug.LogError("Damage called from bullseye");
            _animator.SetTrigger("Hit");
            StartCoroutine(SlowReset());
        }

        private IEnumerator SlowReset()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetTrigger("Restore");
        }
    }
}