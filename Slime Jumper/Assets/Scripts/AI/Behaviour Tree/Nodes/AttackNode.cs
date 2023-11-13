using System.Collections;
using UnityEngine;

public class AttackNode : Node
{
    private EnemyBehaviour enemy;
    private Animator anim;
    private float secUntilNextAttack;

    public AttackNode(EnemyBehaviour enemy, Enemy enemyStats)
    {
        this.enemy = enemy;
        secUntilNextAttack = enemyStats.secUntilNextAttack;
        anim = enemy.anim;
    }

    public override NodeState Evaluate()
    {
        enemy.state = State.Attacking;
        if (enemy.canAttack)
        {
            enemy.canAttack = false;
            Debug.Log("attacking sequence start");

            enemy.StartCoroutine(AttackSequence());
        }

        return NodeState.RUNNING;
    }
    
    IEnumerator AttackSequence()
    {
        anim.SetBool("attacking", true);

        int time = 0;
        while (enemy.state == State.Attacking)
        {
            time++;
            Debug.Log("Times" + time);
            if (enemy.inAttackRange)
            {
                //damage player
            }

            yield return new WaitForSeconds(secUntilNextAttack);
            anim.SetTrigger("attack");
        }
    }
}
