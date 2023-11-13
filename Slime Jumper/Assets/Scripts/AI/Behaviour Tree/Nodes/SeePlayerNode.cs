using UnityEngine;

public class SeePlayerNode : Node
{
    private EnemyBehaviour enemy;

    public SeePlayerNode(EnemyBehaviour enemy)
    {
        this.enemy = enemy;
    }
    public override NodeState Evaluate()
    {
        if (!SeesPlayer())
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
    
    private bool SeesPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, -enemy.transform.right, 10, layerMask: 1<<7);
        Debug.DrawRay(enemy.transform.position, -enemy.transform.right * 10, Color.red);
        return hit.collider != null;
    }
    
}
