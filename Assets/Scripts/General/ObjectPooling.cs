using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace General
{
    [System.Serializable]
    public class ObjectPooling<T> : MonoBehaviour where T : Object
    {
        [SerializeField] private T prefab;
        [SerializeField] private int poolAmount;
        [SerializeField] private List<T> pool;
    
        public void Init(int amount)
        {
            poolAmount = amount;
            for (int i = 0; i < poolAmount; i++)
            {
                pool.Add(Instantiate<T>(prefab, transform.position, Quaternion.identity));
            }
        }

        public T Instantiate()
        {
            // if (GetPooledElemenet() != null)
            // {
            //     return 
            // }
        }
        
        public T GetPooledElemenet()
        {
            for (int i = 0; i < poolAmount; i++)
            {
                if (pool[i].GameObject().activeInHierarchy)
                {
                    return pool[i];
                }
            }
            return null;
        }

        public void DeactivateElement(int i) => pool[i].GameObject().SetActive(false);
        public void ActivateElement(int i) => pool[i].GameObject().SetActive(true);
        
    }
   
    
}

