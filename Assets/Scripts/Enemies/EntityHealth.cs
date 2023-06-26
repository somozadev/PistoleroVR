using UnityEngine;

namespace Enemies
{
    public class EntityHealth : MonoBehaviour
    {
        [SerializeField] private float _health = 3;
        [SerializeField] private float _baseHealth = 3;

        public float Health
        {
            get => _health;
            set => _health = value;
        }

        public float BaseHealth
        {
            get => _baseHealth;
            set => _baseHealth = value;
        }

        [SerializeField] private float _dieTime = 3f;
        public float DieTime => _dieTime;

        public void ResetHealth()
        {
            _health = _baseHealth;
        }
    }
}