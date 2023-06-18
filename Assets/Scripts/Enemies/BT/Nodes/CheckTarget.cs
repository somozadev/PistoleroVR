using BehaviourTree;
using UnityEngine;

namespace Enemies.BT.Nodes
{
    public class CheckTarget : Node
    {
        private Transform _target;
        private bool _referenced = false;
        public CheckTarget()
        {
            name = "CheckTarget";
        }

        public override NodeState Evaluate()
        {
            if (!_referenced)
            {
                _target = (Transform)GetData("target");
                _referenced = true;
                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.SUCCESS;
            return state;
        }
    }
}