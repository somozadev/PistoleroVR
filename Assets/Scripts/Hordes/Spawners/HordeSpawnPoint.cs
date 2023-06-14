using System;
using UnityEngine;

namespace Hordes.Spawners
{
    public class HordeSpawnPoint : MonoBehaviour
    {
        public Transform Transform => transform;
        [SerializeField]private int _maxSpaces = 6;
        [SerializeField]private int _currentUsed = 0;
        [SerializeField] private GameObject prefab;

        public void SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
        }


        public bool HasSpace() => _currentUsed < _maxSpaces;

        public GameObject InstantiateNewEntity(Horde horde)
        {
                _currentUsed++;
            return Instantiate(prefab,transform.position, Quaternion.identity, horde.transform);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}