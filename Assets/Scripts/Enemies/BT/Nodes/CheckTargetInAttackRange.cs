using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.BT.Nodes
{
    public class CheckTargetInAttackRange : Node
    {
        private float _attackRange;
        private Transform _target;
        private Entity _entity;
        private NavMeshAgent _agent;
        private bool _referenced;

        public CheckTargetInAttackRange()
        {
            name = "CheckTargetInAttackRange";
        }

        public override NodeState Evaluate()
        {
            if (!_referenced)
            {
                _target = (Transform)GetData("target");
                _entity = (Entity)GetData("entity");
                _agent = _entity.GetComponent<NavMeshAgent>();
                _attackRange = _entity.AttackRange;
                _referenced = true;
                state = NodeState.RUNNING;
                return state;
            }

            if (Vector3.Distance(_agent.transform.position, _target.position) <= _attackRange)
            {
                _entity.AnimateAttack();
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}