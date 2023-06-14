using System.Collections.Generic;

namespace BehaviourTree
{
    public class SelectorNode : Node //acts like an OR logic gate
    {
        public SelectorNode() : base() {}

        public SelectorNode(List<Node> children) : base(children)
        {
            
            name = "[OR] SelectorNode";
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