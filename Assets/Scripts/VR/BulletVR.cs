using System;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

namespace VR
{
    public class BulletVR : MonoBehaviour
    {
        private Rigidbody _rb;
        private float _shootForce;
        [SerializeField] private GameObject _hitParticles;
        public void Initialize(float shootForce, Vector3 direction, Vector3 startPoint)
        {
            transform.position = startPoint;
            _rb = GetComponent<Rigidbody>();
            _shootForce = shootForce;
            _rb.transform.forward = direction;

        }

        private void Update()
        {
            _rb.velocity = _rb.transform.forward * (_shootForce * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            // GameObject hit = Instantiate(_hitParticles, transform.position, quaternion.identity);
            // hit.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            gameObject.SetActive(false);
        }
    }
    
}