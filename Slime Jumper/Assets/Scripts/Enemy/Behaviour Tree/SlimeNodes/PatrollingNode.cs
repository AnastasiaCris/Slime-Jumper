
using UnityEngine;

public class PatrollingNode : Node
{
    public EnemyBehaviour enemy;
    public float movementSpeed;

    public PatrollingNode(EnemyBehaviour enemy, EnemyScriptableObject enemyScriptableObjectStats)
    {
        this.enemy = enemy;
        movementSpeed = enemyScriptableObjectStats.speed;
    }
    public override NodeState Evaluate()
    {
        enemy.ChangeState(State.Patrolling);
        Patrol();
        
        return NodeState.RUNNING;
    }

    private void Patrol()
    {
        if (enemy.TileFinished())
        {
            enemy.transform.Rotate(Vector3.up, 180f);
            enemy.direction *= -1; //change rotation
        }
        enemy.transform.Translate(-Vector2.right * (movementSpeed * Time.deltaTime));
    }
    
}
