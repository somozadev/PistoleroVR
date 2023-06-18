using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.BT.Nodes
{
    public class TaskPursuit : Node
    {
        private  NavMeshAgent _agent;
        private Transform _target;
        private Entity _entity;
        private bool _referenced;

        public TaskPursuit()
        {
            name = "TaskPursuit";
        }


        public override NodeState Evaluate()
        {
            if (!_referenced)
            {
                _target = (Transform)GetData("target");
                _entity = (Entity)GetData("entity");
                _agent = _entity.GetComponent<NavMeshAgent>();
                _referenced = true;
                state = NodeState.FAILURE;
                return state;
            }

            _agent.SetDestination(_target.position);
            state = NodeState.RUNNING;

            if (!(_agent.remainingDistance <= _agent.stoppingDistance)) return state;
            state = NodeState.SUCCESS;
            return state;

        }
    }
}