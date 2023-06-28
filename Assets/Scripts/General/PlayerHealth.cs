using System.Collections;
using Enemies.BT;
using UnityEngine;
using UnityEngine.Rendering;

namespace General
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHp;
        [SerializeField] private int _currentHp;

        [SerializeField] private MovementVR _movement;
        [SerializeField] private Player _player;
        [SerializeField] private PlayerIngameCanvas _ingameCanvas;
        [SerializeField] private GameOverCanvas _gameOverCanvas;
        [SerializeField] private WavesManager _wavesManager;
        [SerializeField] private Volume _hitEffectVolume;
        [SerializeField] private Volume _inmortalVolume;
        private float _waitTime = 0.4f;

        private bool _inmortal = false;

        public Volume HitEffectVolume => _hitEffectVolume;
        
        public void SetInmortal(bool inmortal)
        {
            if (inmortal)
                _inmortalVolume.weight = 1;
            else
                _inmortalVolume.weight = 0;
            _inmortal = inmortal;
        }

        private void Awake()
        {
            _player = GetComponent<Player>();
            _wavesManager = FindObjectOfType<WavesManager>();
            _ingameCanvas = _player.PlayerIngameCanvas;
            _movement = _player.PlayerMovement;
        }


        public void GainHp(int amount)
        {
            _currentHp += amount;
            _ingameCanvas.UpdateEconomy(_currentHp);
        }

        public void Damage(int amount)
        {
            if (_inmortal) return;
            if (_currentHp - amount < 0)
            {
                _currentHp = 0;
                EndOfGame();
            }
            else
                _currentHp -= amount;
            StartCoroutine(HitEffect());
            _ingameCanvas.UpdateHp(_currentHp);
        }

        private void EndOfGame()
        {
            if(_wavesManager == null)
                _wavesManager = FindObjectOfType<WavesManager>();
            _movement.DisableMovement();
            _gameOverCanvas.OnDie(_wavesManager.WaveNumber);
            _player.PlayerInteractionManager.DisableInteraction();
            _player.PlayerData.AddRun();
            _hitEffectVolume.weight = 1;
            _wavesManager.EndGame();

        }

        private IEnumerator HitEffect()
        {
            float elapsedTime = 0f;
            while (elapsedTime < _waitTime)
            {
                elapsedTime += Time.deltaTime;
                _hitEffectVolume.weight = (elapsedTime / _waitTime);
                yield return null;
            }

            _hitEffectVolume.weight = 0;
        }
    }
}