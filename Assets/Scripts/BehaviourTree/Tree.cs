using System;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        
        [Header("Tree")]
        [SerializeField] private Node root = null;

        protected void Start()
        {
            root = SetupTree();
        }

        private void Update()
        {
            root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}