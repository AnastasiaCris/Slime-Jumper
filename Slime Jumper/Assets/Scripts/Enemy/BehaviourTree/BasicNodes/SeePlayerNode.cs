using UnityEngine;

public class SeePlayerNode : Node
{
    private BaseEnemyBehaviour enemy;
    private LayerMask playerLayer;

    public SeePlayerNode(BaseEnemyBehaviour enemy, LayerMask playerLayer)
    {
        this.enemy = enemy;
        this.playerLayer = playerLayer;
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
    /// <summary>
    /// Creates 2 rays that will check for the player in the direction the enemy looks
    /// </summary>
    /// <returns></returns>
    private bool SeesPlayer()
    {
        Vector2 rayPosOnTop = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 1);
        Vector2 rayPosMid = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(rayPosMid, -enemy.transform.right, 100, playerLayer);
        RaycastHit2D hit1 = Physics2D.Raycast(rayPosOnTop, -enemy.transform.right, 100, playerLayer);
        Debug.DrawRay(rayPosMid, -enemy.transform.right * 24, Color.red);
        Debug.DrawRay(rayPosOnTop, -enemy.transform.right * 24, Color.red);

        if(hit.collider != null || hit1.collider != null)
            Debug.Log("player hit");
        return hit.collider != null || hit1.collider != null; //if either hit turn true
    }
    
}
