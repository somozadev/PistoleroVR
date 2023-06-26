using System;
using UnityEngine;

namespace BehaviourTree
{
    [Serializable]
    public abstract class Tree
    {
        [Header("Tree")] [SerializeField] private Node root = null;
        protected bool _canTick;

        public virtual void Init()
        {
            root = SetupTree();
        }

        public void Tick()
        {
            if (_canTick)
                root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}