using System.Collections;
using General;
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

        public GameObject UseToInstantiate(ObjectPooling pool)
        {
            //TODO: CHANGE TO USE POOLING
            _instantiatedGo = pool.GetPooledElement();
            _instantiatedGo.transform.position = transform.position;
            _instantiatedGo.GetComponent<BT.Entity>().ResetForNewUse();
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