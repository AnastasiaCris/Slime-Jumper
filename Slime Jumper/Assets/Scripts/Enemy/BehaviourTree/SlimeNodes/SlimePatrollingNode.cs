
using UnityEngine;

public class SlimePatrollingNode : Node
{
    public SlimeBehaviour Slime;
    public float movementSpeed;

    public SlimePatrollingNode(SlimeBehaviour slime, EnemyScriptableObject enemyScriptableObjectStats)
    {
        this.Slime = slime;
        movementSpeed = enemyScriptableObjectStats.speed;
    }
    public override NodeState Evaluate()
    {
        Slime.ChangeState(State.Patrolling);
        Patrol();
        
        return NodeState.RUNNING;
    }

    private void Patrol()
    {
        if (Slime.TileFinished())
        {
            Slime.transform.Rotate(Vector3.up, 180f);
            Slime.direction *= -1; //change rotation
        }
        Slime.transform.Translate(-Vector2.right * (movementSpeed * Time.deltaTime));
    }
    
}
