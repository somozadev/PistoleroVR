using System;
using UnityEngine;
using System.Collections.Generic;

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
            AddElementToPool(poolAmount, false);
        }

        public List<GameObject> GetPool()
        {
            return pool;
        }

        private void AddElementToPool(int amount, bool lateAdded)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject go = Instantiate(_usedPrefab, transform.position, Quaternion.identity, transform);
                TryResetTrail(go);
                if (!lateAdded)
                    go.name = ($"bulletVR_{i}");
                else
                {
                    poolAmount++;
                    go.name = ($"bulletVR_{poolAmount}");
                }

                go.SetActive(false);
                pool.Add(go);
            }
        }

        private void TryResetTrail(GameObject go)
        {
            if (TryGetComponent(out BulletVR bullet))
            {
                bullet._trail.Clear();
            }
        }

        public GameObject GetPooledElement()
        {
            for (int i = 0; i < poolAmount; i++)
            {
                if (!pool[i].gameObject.activeSelf)
                {
                    Debug.Log($"Pooled element with index {i} of {poolAmount} with state of {pool[i].activeSelf}");
                    TryResetTrail(pool[i]);
                    pool[i].gameObject.SetActive(true);
                    return pool[i];
                }
            }

            Debug.Log("Can't find element in current pool, creating new one");
            AddElementToPool(1, true);
            TryResetTrail(pool[pool.Count - 1]);
            pool[pool.Count - 1].SetActive(true);
            return pool[pool.Count - 1];
        }
    }
}