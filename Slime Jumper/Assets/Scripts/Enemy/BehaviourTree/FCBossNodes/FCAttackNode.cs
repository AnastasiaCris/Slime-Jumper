using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCAttackNode : Node
{
    private FCBossBehaviour boss;
    private Animator anim;
    private List<FCAttackType> attackSequence = new List<FCAttackType>();
    private WaitUntil wait;

    public FCAttackNode(FCBossBehaviour boss, List<FCAttackType> attackSequence)
    {
        this.boss = boss;
        this.attackSequence = attackSequence;
        anim = boss.Anim;
        
        wait = new WaitUntil(() => boss.animEnd);
    }
    public override NodeState Evaluate()
    {
        boss.ChangeState(State.Attacking);
        if (boss.canAttack && boss.InAttackRange && boss.hasIdled)
        {
            anim.SetBool(boss.idleAnimHash, false);
            boss.SetHasIdled(false);
            boss.SetIdleFinished(false);
            boss.SetHasAttacked(false);
            boss.SetCanAttack(false);
            boss.SetIsAttacking(true);
            boss.StopCoroutine(AttackSequence());
            boss.StartCoroutine(AttackSequence());
        }
        
        return boss.State == State.Attacking ? NodeState.RUNNING : NodeState.FAILURE;
    }

    IEnumerator AttackSequence()
    {
        for (int i = 0; i < attackSequence.Count; i++)
        {
            boss.CheckFacingPlayer();
            if (attackSequence[i] == FCAttackType.Attack)
            {
                anim.SetTrigger(boss.attackAnimHash);
                
                if(i+1 < attackSequence.Count-1 && attackSequence[i+1] == FCAttackType.FakeRecovery)
                    anim.SetBool(boss.fakeRecoveryAnimHash, true);
                else
                    anim.SetBool(boss.fakeRecoveryAnimHash, false);
                    
            }

            yield return wait;
            boss.SetAnimState(false);
        } 
        yield return new WaitForSeconds(0.5f);
        boss.SetIsAttacking(false);
        boss.SetHasAttacked(true);
        boss.SetCanAttack(true);
        anim.SetBool(boss.idleAnimHash, true); 
    }

}
