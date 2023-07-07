using System.Collections.Generic;
using System.Linq;
using General;
// using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace Enemies.BT
{
    public class EntitiesManager : MonoBehaviour
    {
        public int currentEntities;
        private int _savedToTryLaterEntitiesForNoSpace;
        public int waveMaxEntities;

        [SerializeField] private ObjectPooling _entitiesPooling;
        
        
        private EntitySpawnPoint[] _spawnPoints;
        [SerializeField] private List<Entity> _entities;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _joinRange = 6f;
        public List<Entity> Entities => _entities;
        public float JoinRange => _joinRange;

        [SerializeField] private int _waveNumber;

        public void CheckIfNoMoreEntities()
        {
            if(_entities.Count<=0)
            {
                GameManager.Instance.players[0].CountdownPlayerCanvas.SetCountdownNumber(5);
                GameManager.Instance.players[0].CountdownPlayerCanvas.CurrentWave = _waveNumber;
                GameManager.Instance.players[0].CountdownPlayerCanvas.NewWave();
            }
        }

        private void Awake()
        {
            _entitiesPooling = GameManager.Instance.objectPoolingManager.GetNewObjectPool("entititesPooling", ref _prefab, 5);
            _spawnPoints = GetComponentsInChildren<EntitySpawnPoint>();
        }

        public void SetupForNewWave(int waveNumber)
        {
            _waveNumber = waveNumber;
            waveMaxEntities = (int)(waveNumber * 1.2310f) + 2;
            
            
        }

        public void InstantiateEntities()
        {
            for (currentEntities = 0; currentEntities < waveMaxEntities; currentEntities++)
                InstantiateEntity();
        }

        private EntitySpawnPoint GetRandomAvailableSpawnPoint()
        {
            Random random = new Random();
            // var availableSpawnPoints = _spawnPoints.Where(sp => !sp.IsInUse()).ToArray();
            if (_spawnPoints.Length > 0)
            {
                int randomIndex = random.Next(0, _spawnPoints.Length);
                return _spawnPoints[randomIndex];
            }

            return null;
        }

        private void InstantiateEntity()
        {
            if (currentEntities >= waveMaxEntities) return;
            EntitySpawnPoint selected = GetRandomAvailableSpawnPoint();
            if (selected != null)
            {
                var entity = selected.UseToInstantiate(_entitiesPooling).GetComponent<Entity>();
                entity.AssignWavesManager(this);
                entity.EntityHealth.BaseHealth += _waveNumber/5f;
                _entities.Add(entity);
            }
            else
            {
                Debug.LogWarning("<color'orange'> NO SPAWNER AVAILABLE </color>");
            }
        }
    }
}