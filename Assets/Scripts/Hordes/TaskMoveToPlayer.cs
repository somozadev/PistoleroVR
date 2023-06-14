using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace Hordes
{
    public class TaskMoveToPlayer : Node
    {
        private static GameObject _target;

        public TaskMoveToPlayer(GameObject target)
        {
            name = "TaskMoveToPlayer";
            _target = target;
        }

        public override NodeState Evaluate()
        {
            var horde = (Horde)GetData("currentHorde");
            Debug.Log("Tracking shit");
            if (!ReferenceEquals(horde, null))
            {
                horde.Agent.SetDestination(_target.transform.position);
                horde.MoveGroupAlong();
                state = NodeState.RUNNING;
                return state;
            }
            horde.Agent.ResetPath();
            state = NodeState.FAILURE;
            return state;
        }
    }
}