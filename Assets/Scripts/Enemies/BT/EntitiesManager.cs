using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace Enemies.BT
{
    public class EntitiesManager : MonoBehaviour
    {
        public int currentEntities;
        private int _savedToTryLaterEntitiesForNoSpace;
        public int waveMaxEntities;

        private EntitySpawnPoint[] _spawnPoints;
        [SerializeField] private List<Entity> _entities;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _joinRange = 6f;
        public List<Entity> Entities => _entities;
        public float JoinRange => _joinRange;

        private void Awake()
        {
            _spawnPoints = GetComponentsInChildren<EntitySpawnPoint>();
        }

        public void SetupForNewWave(int waveNumber)
        {
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
            var availableSpawnPoints = _spawnPoints.Where(sp => !sp.IsInUse()).ToArray();
            if (availableSpawnPoints.Length > 0)
            {
                int randomIndex = random.Next(0, availableSpawnPoints.Length);
                return availableSpawnPoints[randomIndex];
            }

            return null;
        }

        private void InstantiateEntity()
        {
            if (currentEntities >= waveMaxEntities) return;
            EntitySpawnPoint selected = GetRandomAvailableSpawnPoint();
            if (selected != null)
            {
                _entities.Add(selected.UseToInstantiate(_prefab).GetComponent<Entity>());
            }
            else
            {
                Debug.LogWarning("<color'orange'> NO SPAWNER AVAILABLE </color>");
            }
        }
    }
}