
using UnityEngine;

public class SlimePatrollingNode : Node
{
    private SlimeBehaviour slime;
    private float movementSpeed;


    public SlimePatrollingNode(SlimeBehaviour slime, EnemyScriptableObject enemyScriptableObjectStats)
    {
        this.slime = slime;
        movementSpeed = enemyScriptableObjectStats.speed;
    }
    public override NodeState Evaluate()
    {
        slime.ChangeState(State.Patrolling);
        Patrol();
        
        if(slime.State == State.Patrolling)return NodeState.RUNNING;
        return NodeState.FAILURE;
    }

    private void Patrol()
    {
        if (slime.TileFinished())
        {
            slime.transform.Rotate(Vector3.up, 180f);
            slime.direction *= -1; //change rotation
        }
        slime.transform.Translate(-Vector2.right * (movementSpeed * Time.deltaTime));
    }
    
}
