using UnityEngine;

namespace General.Damageable
{
    public class NormalTarget : Damageable
    {
        [SerializeField] private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
    }
}