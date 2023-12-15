using System.Collections;
using UnityEngine;

public class AttackNode : Node
{
    private EnemyBehaviour enemy;
    private Player player;
    private Animator anim;

    private float secUntilNextAttack;

    public AttackNode(EnemyBehaviour enemy, EnemyScriptableObject enemyScriptableObjectStats)
    {
        this.enemy = enemy;
        secUntilNextAttack = enemyScriptableObjectStats.secUntilNextAttack;
        anim = enemy.Anim;
    }

    public override NodeState Evaluate()
    {
        enemy.ChangeState(State.Attacking);
        if (enemy.canAttack)
        {
            enemy.canAttack = false;
            enemy.StopCoroutine(AttackSequence());
            enemy.StartCoroutine(AttackSequence());
        }

        return NodeState.RUNNING;
    }
    
    IEnumerator AttackSequence()
    {
        anim.SetBool("attacking", true);

        while (enemy.State == State.Attacking)
        {
            anim.SetTrigger("attack");
            
            yield return new WaitForSeconds(secUntilNextAttack);
        }
    }
    
}
