using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.BT.Nodes
{
    public class CheckEntityIsClose : Node
    {
        private EntitiesManager _entitiesManager;
        private Entity _entity;
        private bool _referenced;

        public CheckEntityIsClose()
        {
            name = "CheckEntityIsClose";
        }

        public override NodeState Evaluate()
        {
            if (!_referenced)
            {
                _entitiesManager = (EntitiesManager)GetData("entitiesManager");
                _entity = (Entity)GetData("entity");
                _referenced = true;
                state = NodeState.FAILURE;
                return state;
            }

            foreach (var entity in _entitiesManager.Entities)
            {
                if (entity != _entity)
                {
                    if (Vector3.Distance(_entity.transform.position, entity.transform.position) <=
                        _entitiesManager.JoinRange)
                    {
                        parent.SetData("joinableEntity", entity);
                        state = NodeState.SUCCESS;
                        return state;
                    }
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}