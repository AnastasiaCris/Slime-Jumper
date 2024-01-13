[System.Serializable]
public abstract class Node
{
    protected NodeState _nodeState;
    public NodeState NodeState {get {return _nodeState;}}

    public abstract NodeState Evaluate();
}

public enum NodeState
{
    SUCCESS, RUNNING, FAILURE,
}
