using System;
using System.Collections.Generic;
using BehaviourTree;
using Hordes.Spawners;
using UnityEngine;
using Tree = BehaviourTree.Tree;

namespace Hordes
{
    public class HordeBT : Tree
    {
        [Space(20)] [Header("Variables")]
        [SerializeField] private float _waitToInstantiateTime;
        [Space(20)]
        [Header("References")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private HordeSpawnPoints _spawnPoints;
        [SerializeField] private Horde _horde;
        [SerializeField] private WavesManager _wavesManager;
        
        private void Awake()
        {
            _horde = GetComponent<Horde>();
            _wavesManager = GetComponentInParent<WavesManager>();
        }

        protected override Node SetupTree()
        {
            
            
            Node instantiateNewEntityNode = new SequenceNode(new List<Node>
            {
                new CheckWaveSupportsMoreEntities(_wavesManager),
                new CheckHordeHasSpace(_horde),
                new CheckSpawnerAvailable(_spawnPoints.SpawnPoints),
                new TaskInstantiate(_waitToInstantiateTime)
            });

            Node moveHordeNode = new SequenceNode(new List<Node>
            {
                new CheckHordeHasEnoughEntities(),
                new CheckHordeIsGrouped(),
                new TaskMoveToPlayer(_horde.Target)
            });

            Node joinHordeNode = new SequenceNode(new List<Node>
            {

            });
            
            Node root = new SelectorNode(new List<Node>
            {
                instantiateNewEntityNode,
                moveHordeNode,
                // joinHordeNode
                

            });
            
           
            return root;
        }
    }

}