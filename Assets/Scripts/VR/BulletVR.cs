using System;
using System.Collections;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

namespace VR
{
    public class BulletVR : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _bulletTrail;
        [SerializeField] private ParticleSystem _impactParticle;

        public void Initialize(RaycastHit hit, Vector3 startPoint)
        {
            transform.position = startPoint;
            _bulletTrail.enabled = true;
            // TrailRenderer trail = Instantiate(_bulletTrail, startPoint, Quaternion.identity);
            StartCoroutine(SpawnTrail(_bulletTrail, hit));
        }

        private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
        {
            float time = 0;
            Vector3 startPos = transform.position;
            while (time < 1)
            {
                transform.position = Vector3.Lerp(startPos, hit.point, time);
                time += Time.deltaTime / trail.time;
                yield return null;
            }

            transform.position = hit.point;
            Instantiate(_impactParticle, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(trail.gameObject, trail.time);
        }
      
    }
}