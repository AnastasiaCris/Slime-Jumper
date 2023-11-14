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
            enemy.StartCoroutine(AttackSequence());
        }

        return NodeState.RUNNING;
    }
    
    IEnumerator AttackSequence()
    {
        //Debug.Log("Starting Attack Coroutine");
        anim.SetBool("attacking", true);

        int time = 0;
        while (enemy.state == State.Attacking)
        {
            time++;
            //Debug.Log("Times" + time);
            anim.SetTrigger("attack");
            if (enemy.inAttackRange)
            {
                //damage player
            }

            yield return new WaitForSeconds(secUntilNextAttack);
            //Debug.Log("Times atfer wait" + time);
        }
        
        //Debug.Log("Ending Attack Coroutine");
    }
}
