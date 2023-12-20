using System.Collections;
using UnityEngine;

public class SlimeAttackNode : Node
{
    private SlimeBehaviour _slime;
    private Player player;
    private Animator anim;

    private float secUntilNextAttack;

    public SlimeAttackNode(SlimeBehaviour slime, EnemyScriptableObject enemyScriptableObjectStats)
    {
        this._slime = slime;
        secUntilNextAttack = enemyScriptableObjectStats.secUntilNextAttack;
        anim = slime.Anim;
    }

    public override NodeState Evaluate()
    {
        _slime.ChangeState(State.Attacking);
        if (_slime.canAttack)
        {
            _slime.canAttack = false;
            _slime.StopCoroutine(AttackSequence());
            _slime.StartCoroutine(AttackSequence());
        }

        return NodeState.RUNNING;
    }
    
    IEnumerator AttackSequence()
    {
        anim.SetBool("attacking", true);

        while (_slime.State == State.Attacking)
        {
            anim.SetTrigger("attack");
            
            yield return new WaitForSeconds(secUntilNextAttack);
        }
    }
    
}
