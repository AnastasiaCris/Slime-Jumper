using System.Collections;
using UnityEngine;

public class SlimeAttackNode : Node
{
    private SlimeBehaviour slime;
    private Player player;
    private Animator anim;

    private float secUntilNextAttack;

    public SlimeAttackNode(SlimeBehaviour slime, EnemyScriptableObject enemyScriptableObjectStats)
    {
        this.slime = slime;
        secUntilNextAttack = enemyScriptableObjectStats.secUntilNextAttack;
        anim = slime.Anim;
    }

    public override NodeState Evaluate()
    {
        slime.ChangeState(State.Attacking);
        if (slime.canAttack)
        {
            slime.canAttack = false;
            slime.StopCoroutine(AttackSequence());
            slime.StartCoroutine(AttackSequence());
        }

        if(slime.State == State.Attacking) return NodeState.RUNNING;
        return NodeState.FAILURE;
    }
    
    IEnumerator AttackSequence()
    {
        anim.SetBool("attacking", true);

        while (slime.State == State.Attacking)
        {
            anim.SetTrigger("attack");
            
            yield return new WaitForSeconds(secUntilNextAttack);
        }
    }
    
}
