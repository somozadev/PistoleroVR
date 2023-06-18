using System;
using UnityEngine;

namespace BehaviourTree
{
    [Serializable]
    public abstract class Tree
    {
        [Header("Tree")] [SerializeField] private Node root = null;

        public virtual void Init()
        {
            root = SetupTree();
        }

        public void Tick()
        {
            root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}