using System.Collections.Generic;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence() : base () { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            //bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                        //anyChildIsRunning = true;
                        //continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = NodeState.FAILURE;
            //state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}
