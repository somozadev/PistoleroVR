using BehaviourTree;

namespace Enemies.BT.Nodes
{
    public class TaskAttack : Node
    {
        private bool _referenced;
        private Entity _entity;

        public TaskAttack()
        {
            name = "TaskAttack";
        }

        public override NodeState Evaluate()
        {
            if (!_referenced)
            {
                _entity = (Entity)GetData("entity");
                _referenced = true;
                state = NodeState.FAILURE;
                return state;
            }

            _entity.Attack();
            state = NodeState.SUCCESS;
            return state;

        }
    }
}