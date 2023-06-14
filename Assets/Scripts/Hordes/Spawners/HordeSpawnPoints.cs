using System;
using UnityEngine;

namespace Hordes.Spawners
{
    public class HordeSpawnPoints : MonoBehaviour
    {
        [SerializeField] private HordeSpawnPoint[] _spawnPoints;
        [SerializeField] private GameObject _prefab;

        public HordeSpawnPoint[] SpawnPoints => _spawnPoints;
        private void Awake()
        {
            foreach (var spawnPoint in _spawnPoints)
            {
                spawnPoint.SetPrefab(_prefab);
            }
        }
    }
}