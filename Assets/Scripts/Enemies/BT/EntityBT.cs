using System;
using System.Collections.Generic;
using BehaviourTree;
using Enemies.BT.Nodes;
// using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviourTree.Tree;

namespace Enemies.BT
{
    [Serializable]
    public class EntityBT : Tree
    {
        private Transform _target;
        private NavMeshAgent _agent;
        private EntitiesManager _entitiesManager;

        public void Init(Transform target, NavMeshAgent agent, EntitiesManager entitiesManager)
        {
            _target = target;
            _agent = agent;
            _entitiesManager = entitiesManager;
            base.Init();
        }

        public void StopTree()
        {
            _canTick = false;
        }

        public void StartTree()
        {
            _canTick = true;
        }

        protected override Node SetupTree()
        {
            Node root = new SelectorNode("Root", _agent.GetComponent<Entity>(), _target, _entitiesManager,
                new List<Node>
                {
                    new SequenceNode("AttackSequence", new List<Node>
                    {
                        new CheckTargetInAttackRange(),
                        new TaskAttack()
                    }),
                    new SequenceNode("PursuitSequence", new List<Node>
                    {
                        new CheckTarget(),
                        new TaskPursuit()
                    }),
                    new SequenceNode("JoinSequence", new List<Node>
                    {
                        new CheckEntityIsClose(),
                        new TaskJoin()
                    }),
                });
            return root;
        }
    }
}