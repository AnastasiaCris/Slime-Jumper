using System.Collections;
using UnityEngine;

public class AttackNode : Node
{
    private EnemyBehaviour enemy;
    private Player player;
    private Animator anim;

    private float secUntilNextAttack;

    public AttackNode(EnemyBehaviour enemy, Enemy enemyStats)
    {
        this.enemy = enemy;
        this.player = player;
        secUntilNextAttack = enemyStats.secUntilNextAttack;
        anim = enemy.anim;
    }

    public override NodeState Evaluate()
    {
        enemy.state = State.Attacking;
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

        while (enemy.state == State.Attacking)
        {
            anim.SetTrigger("attack");
            
            yield return new WaitForSeconds(secUntilNextAttack);
        }
    }
    
}
