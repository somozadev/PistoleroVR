using System.Collections.Generic;
using Enemies.BT;
using UnityEngine;

namespace BehaviourTree
{
    public sealed class SelectorNode : Node //acts like an OR logic gate
    {
        public SelectorNode() : base()
        {
        }

        public SelectorNode(string name, List<Node> children) : base(children)
        {
            this.name = name;
        }

        public SelectorNode(List<Node> children) : base(children)
        {
            name = "[OR] SelectorNode";
        }

        public SelectorNode(string name, Entity entity, Transform target, EntitiesManager entitiesManager , List<Node> children) : base(children)
        {
            this.name = name;
            SetData("entity",entity);
            SetData("target",target);
            SetData("entitiesManager",entitiesManager);
        }


        public override NodeState Evaluate()
        {
            foreach (Node child in children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}