using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCAttackNode : Node
{
    private FCBossBehaviour boss;
    private Animator anim;
    private List<int> attackSequence = new List<int>();
    private WaitUntil wait;

    public FCAttackNode(FCBossBehaviour boss, List<int> attackSequence)
    {
        this.boss = boss;
        this.attackSequence = attackSequence;
        anim = boss.Anim;
        
        wait = new WaitUntil(boss.GetAnimState);
    }
    public override NodeState Evaluate()
    {
        if (boss.canAttack)
        {
            boss.canAttack = false;
            boss.StopCoroutine(AttackSequence());
            boss.StartCoroutine(AttackSequence());
        }
        return NodeState.RUNNING;
    }

    IEnumerator AttackSequence()
    {
        for (int i = 0; i < attackSequence.Count; i++)
        {
            if (anim.GetParameter(attackSequence[i]).type == AnimatorControllerParameterType.Trigger)
            {
                anim.SetTrigger(attackSequence[i]);
            }
            else if (anim.GetParameter(attackSequence[i]).type == AnimatorControllerParameterType.Bool)
            {
                anim.SetBool(attackSequence[i], true);
            }

            yield return wait;
            boss.SetAnimState(false);
        } 
    }

}
