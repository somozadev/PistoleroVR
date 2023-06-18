using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

namespace Enemies.BT.Nodes
{
    public class TaskJoin : Node
    {
        private Entity _joinableEntity;
        private NavMeshAgent _agent;
        private bool _referenced;

        public TaskJoin()
        {
            name = "TaskJoin";
        }

        public override NodeState Evaluate()
        {
            if (!_referenced)
            {
                _agent = ((Entity)GetData("entity")).GetComponent<NavMeshAgent>();
                _joinableEntity = (Entity)GetData("joinableEntity");
                _referenced = true;
                state = NodeState.FAILURE;
                return state;
            }

            _joinableEntity = (Entity)GetData("joinableEntity");
            if (ReferenceEquals(_joinableEntity, null))
            {
                state = NodeState.FAILURE;
                return state;
            }

            _agent.SetDestination(_joinableEntity.transform.position);
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}