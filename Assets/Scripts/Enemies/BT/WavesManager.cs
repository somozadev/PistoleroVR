using System;
using System.Collections;
using General;
using UnityEngine;

namespace Enemies.BT
{
    public class WavesManager : MonoBehaviour
    {
        [SerializeField] private int _wave = 1;
        
        private EntitiesManager _entitiesManager;

        public Transform target;

        public int WaveNumber => _wave;
        public EntitiesManager EntitiesManager => _entitiesManager;
        
        private void Awake()
        {
            _entitiesManager = GetComponentInChildren<EntitiesManager>();
        }

        private void OnEnable()
        {
            EventManager.NewWave+= StartNewWave;
        }

        private void OnDisable()
        {
            EventManager.NewWave-= StartNewWave;
        }

        public void EndGame()
        {
            GameManager.Instance.objectPoolingManager.DeleteObjectPooling("entititesPooling");
            Destroy(_entitiesManager.gameObject);
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