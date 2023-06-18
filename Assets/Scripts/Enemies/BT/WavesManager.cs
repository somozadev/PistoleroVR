using System;
using UnityEngine;

namespace Enemies.BT
{
    public class WavesManager : MonoBehaviour
    {
        [SerializeField] private int _wave = 1;
        
        private EntitiesManager _entitiesManager;

        public delegate void NewWave();

        public event NewWave OnNewWave;

        public Transform target;
        private void Awake()
        {
            _entitiesManager = GetComponentInChildren<EntitiesManager>();
        }

        private void Start()
        {
            _entitiesManager.SetupForNewWave(_wave);
            _entitiesManager.InstantiateEntities();
        }
        
        [ContextMenu("StartNewWave")]
        private void StartNewWave()
        {
            _wave++;
            _entitiesManager.SetupForNewWave(_wave);
            _entitiesManager.InstantiateEntities();
        }
    }
}