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
        Vector2 rayPosOnTop = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, -enemy.transform.right, 100, layerMask: 1<<7);
        RaycastHit2D hit1 = Physics2D.Raycast(rayPosOnTop, -enemy.transform.right, 100, layerMask: 1<<7);
        Debug.DrawRay(enemy.transform.position, -enemy.transform.right * 24, Color.red);
        Debug.DrawRay(rayPosOnTop, -enemy.transform.right * 24, Color.red);

        return hit.collider != null || hit1.collider != null; //if either hit turn true
    }
    
}
