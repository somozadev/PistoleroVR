using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;

namespace General
{
    public class ObjectPooling : MonoBehaviour
    {
        [SerializeField] private string poolName;
        private int poolId;
        [SerializeField] private int poolAmount;
        [SerializeField] private List<GameObject> pool;
        private GameObject _usedPrefab;
        public void Init(string name, int id, int amount, ref GameObject prefab)
        {
            _usedPrefab = prefab;
            pool = new List<GameObject>();
            poolName = name;
            poolId = id;
            poolAmount = amount;
            AddElementToPool(poolAmount);
        }

        private void AddElementToPool(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject go = Instantiate(_usedPrefab, transform.position, Quaternion.identity,  transform);
                go.SetActive(false);
                pool.Add(go);
            }
        }

        public GameObject GetPooledElement(Transform pivot)
        {

            
            for (int i = 0; i < poolAmount; i++)
            {
                if (!pool[i].GameObject().activeSelf)
                {
                    pool[i].GameObject().SetActive(true);
                    return pool[i];
                }
            }

            AddElementToPool(1);
            pool[pool.Count - 1].SetActive(true);
            return pool[pool.Count - 1];
        }
    }
}