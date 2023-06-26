using UnityEngine;

namespace General
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHp;
        [SerializeField] private int _currentHp;

        [SerializeField] private PlayerIngameCanvas _ingameCanvas;


        private void Awake()
        {
            _ingameCanvas = GetComponentInChildren<PlayerIngameCanvas>();
        }


        public void GainHp(int amount)
        {
            _currentHp += amount;
            _ingameCanvas.UpdateEconomy(_currentHp);
        }

        public void Damage(int amount)
        {
            if (_currentHp - amount < 0)
                _currentHp = 0;
            else
                _currentHp -= amount;
            _ingameCanvas.UpdateHp(_currentHp);
        }
    }
}