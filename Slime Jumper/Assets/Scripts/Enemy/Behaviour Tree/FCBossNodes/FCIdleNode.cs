using System.Collections;
using UnityEngine;

public class FCIdleNode : Node
{
    private float secStayingIdle;
    private NodeState node;

    public FCIdleNode(float sec)
    {
        secStayingIdle = sec;
    }
    public override NodeState Evaluate()
    {
        return node;
    }

    private IEnumerator Wait()
    {
        node = NodeState.SUCCESS;
        
        yield return new WaitForSeconds(secStayingIdle);

        node = NodeState.FAILURE;
    }
}
