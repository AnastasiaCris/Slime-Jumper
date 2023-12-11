
using UnityEngine;

public class StartedChasing : Node
{
    private EnemyBehaviour enemy;
    private Animator anim;

    public StartedChasing(EnemyBehaviour enemy)
    {
        this.enemy = enemy;
        anim = enemy.anim;
    }
    public override NodeState Evaluate()
    {
        if (!enemy.chasing)
        {
            anim.SetTrigger("patroll");
        }
        return enemy.chasing ? NodeState.FAILURE : NodeState.SUCCESS;
    }
}
