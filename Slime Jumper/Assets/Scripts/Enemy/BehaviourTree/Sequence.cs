using System.Collections.Generic;
/// <summary>
/// When any node returns failure the sequence is a failure
/// When all nodes are successful the sequence is successful
/// </summary>
public class Sequence : Node
{
    protected List<Node> nodes = new List<Node>();

    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    
    public override NodeState Evaluate()
    {
        bool isAnyNodeRunning = false;
        foreach (var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    isAnyNodeRunning = true;
                    break;
                case NodeState.SUCCESS:
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
            }
        }

        _nodeState = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;

        return _nodeState;
    }
}
