using System.Collections.Generic;
namespace BehaviourTree
{
    public class SequenceNode : Node //acts like an AND logic gate
    {
        public SequenceNode() : base() {}

        public SequenceNode(List<Node> children) : base(children){
            name = "[AND] SequenceNode";
            
        }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;
            foreach (var child in children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}