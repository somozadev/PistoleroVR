using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Node
    {
        protected string name;
        protected NodeState state; //the current node state
        public Node parent;//the parent nodes from this one
        public List<Node> children = new List<Node>();//the children nodes from this one
        private Dictionary<string, object> _data = new Dictionary<string, object>(); //the shared data

        public Node() { parent = null; }

        public Node(List<Node> children)
        {
            foreach (var child in children)
                Attach(child);
        }
        private void Attach(Node node)
        {
            node.parent = this; 
            children.Add(node);
        }
       
       
        public virtual void SetData(string key, object value) { _data[key] = value; }
        public object GetData(string key) //recursively looks for the data in parent nodes
        {
            object val = null;
            if (_data.TryGetValue(key, out val))
                return val;

            Node node = parent;
            if (node != null)
                val = node.GetData(key);
            return val;
        }
        public bool ClearData(string key) //recursively looks for the data to remove and removes it
        {
            bool cleared = false;
            if (_data.ContainsKey(key))
            {
                _data.Remove(key);
                return true;
            }

            Node node = parent;
            if (node != null)
                cleared = node.ClearData(key);
            return cleared;

        }
        
        
        public virtual NodeState Evaluate() => NodeState.FAILURE;
    }
}