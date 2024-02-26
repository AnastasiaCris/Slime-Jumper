using System.Collections;
using UnityEngine;

public class FCMSkeletonAttackNode : Node
{

    private FCMSkeletonBehaviour enemy;
    private Player player;
    private Animator anim;

    private float secUntilNextAttack;

    public FCMSkeletonAttackNode(FCMSkeletonBehaviour enemy, EnemyScriptableObject enemyScriptableObjectStats)
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

        if(enemy.State == State.Attacking) return NodeState.RUNNING;
        return NodeState.FAILURE;
    }
    
    IEnumerator AttackSequence()
    {
        int random = Random.Range(1, 3);

        while (enemy.State == State.Attacking)
        {
            random = Random.Range(1, 3);
            switch (random)
            {
                case 1:
                    anim.SetTrigger(enemy.attack1AnimHash);
                    break;
                case 2:
                     anim.SetTrigger(enemy.attack2AnimHash);
                     break;
            }
            
            yield return new WaitForSeconds(secUntilNextAttack);
        }
    }

}
