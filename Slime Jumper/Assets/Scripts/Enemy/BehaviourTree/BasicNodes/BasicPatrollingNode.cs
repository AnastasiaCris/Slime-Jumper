
using UnityEngine;

public class BasicPatrollingNode : Node
{
    private BaseEnemyBehaviour enemy;
    private float movementSpeed;
    private LayerMask tileLayer;


    public BasicPatrollingNode(BaseEnemyBehaviour enemy, EnemyScriptableObject enemyScriptableObjectStats, LayerMask tileLayer)
    {
        this.enemy = enemy;
        this.tileLayer = tileLayer;
        movementSpeed = enemyScriptableObjectStats.speed;
    }
    public override NodeState Evaluate()
    {
        enemy.ChangeState(State.Patrolling);
        Patrol();
        
        if(enemy.State == State.Patrolling)return NodeState.RUNNING;
        return NodeState.FAILURE;
    }

    private void Patrol()
    {
        if (TileFinished())
        {
            enemy.transform.Rotate(Vector3.up, 180f);
            enemy.direction *= -1; //change rotation
        }
        enemy.transform.Translate(-Vector2.right * (movementSpeed * Time.deltaTime));
    }
    
    private bool TileFinished()
    {
        Vector2 boxcastOrigin = enemy.direction == 1 ? new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y - 0.5f) : new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.BoxCast(boxcastOrigin, new Vector2(1, 0.5f), 0f, Vector2.right * enemy.direction, 0f, tileLayer);

        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }
    
}
