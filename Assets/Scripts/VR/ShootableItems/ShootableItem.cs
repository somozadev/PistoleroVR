using System;
using System.Collections;
using General.Damageable;
using UnityEngine;

namespace VR.ShootableItems
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShootableItem : Damageable
    {
        [SerializeField] private float _waitSeconds;
        [SerializeField] private Rigidbody _rb;
        private Vector3 _initialPos;
        private Quaternion _initialRot; 

        private void Awake()
        {
            var trf = transform;
            _initialPos = trf.position;
            _initialRot = trf.rotation;
            // tag = "Damageable";
            _rb = GetComponent<Rigidbody>();
        }
        public override void Damage(RevolverVR revolverVR)
        {
            StartCoroutine(RestockSelf());
        }

        private IEnumerator RestockSelf()
        {
            yield return new WaitForSecondsRealtime(_waitSeconds);
            var trf = transform;
            _rb.ResetInertiaTensor();
            _rb.velocity = Vector3.zero;
            trf.position = _initialPos;
            trf.rotation = _initialRot;
        }
    }
}