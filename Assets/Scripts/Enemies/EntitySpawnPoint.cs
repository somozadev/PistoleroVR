using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EntitySpawnPoint : MonoBehaviour
    {
        [SerializeField] private bool _used;
        [SerializeField] private float _waitToCheckTime = 0.5f;
        private GameObject _instantiatedGo;
        private const float SpawnRange = 2f;

        public bool IsInUse() => _used;
        public GameObject UseToInstantiate(GameObject prefab)
        { //TODO: CHANGE TO USE POOLING
            _instantiatedGo = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
            // _used = true;
            StartCoroutine(WaitInstantiatedToLeave());
            return _instantiatedGo;
        }

        private IEnumerator WaitInstantiatedToLeave()
        {
            while (Vector3.Distance(_instantiatedGo.transform.position, transform.position) <= SpawnRange)
                yield return new WaitForSeconds(_waitToCheckTime);
            _used = false;
            yield return null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, SpawnRange);
        }
    }
}