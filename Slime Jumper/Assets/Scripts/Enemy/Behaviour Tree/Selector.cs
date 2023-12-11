using System.Collections.Generic;

/// <summary>
/// When all nodes returned failure the selector is a failure
/// When at least one node is successful the selector is successful
/// </summary>
public class Selector : Node
{
    protected List<Node> nodes = new List<Node>();

    public Selector(List<Node> nodes)
    {
        this.nodes = nodes;
    }
    
    public override NodeState Evaluate()
    {
        foreach (var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    return _nodeState;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
                case NodeState.FAILURE:
                    break;
            }
        }

        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }
}
