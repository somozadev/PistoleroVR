using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class ObjectPoolingManager : MonoBehaviour
    {
        public Dictionary<string, ObjectPooling> objectPools = new Dictionary<string, ObjectPooling>();
        private int ids = 0;

        public void NewObjectPool(string name, ref GameObject prefab, int amount)
        {
            ObjectPooling pool = new GameObject(name, typeof(ObjectPooling)).GetComponent<ObjectPooling>();
            pool.transform.SetParent(transform);
            pool.Init(name, ids, amount, ref prefab);
            ids++;
            objectPools.Add(name, pool);
        }

        public ObjectPooling GetNewObjectPool(string name, ref GameObject prefab, int amount)
        {
            ObjectPooling pool = new GameObject(name, typeof(ObjectPooling)).GetComponent<ObjectPooling>();
            pool.transform.SetParent(transform);
            pool.Init(name, ids, amount, ref prefab);
            ids++;
            objectPools.Add(name, pool);
            return pool;
        }

        public ObjectPooling GetPoolByName(string name)
        {
            return objectPools[name];
        }
    }
}