using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Node
{
    private FCBossBehaviour boss;
    private float timeToIdle;
    private bool idleStarted;
    private float playerRange;

    private Transform enemy;
    private Transform target;
    public IdleNode(FCBossBehaviour boss, float timeToIdle, float playerRange)
    {
        this.boss = boss;
        this.timeToIdle = timeToIdle;
        this.playerRange = playerRange;

        enemy = boss.transform;
        target = GameProperties.playerLoc;
    }

    public override NodeState Evaluate()
    {
        boss.ChangeState(State.Idle);
        if (!boss.idleFinished && !idleStarted && !boss.isAttacking)
        {
            idleStarted = true;
            boss.StartCoroutine(StartIdle());
        }
        
        return !boss.idleFinished ? NodeState.RUNNING : NodeState.FAILURE;
    }

    private IEnumerator StartIdle()
    {
        boss.SetHasIdled(false);
        yield return new WaitForSeconds(timeToIdle);
        boss.SetHasIdled(true);
        boss.SetIdleFinished(true);
        idleStarted = false;
        
        float distance = Vector2.Distance(target.position, enemy.position);
        float yDist = target.position.y - enemy.position.y;
        if (yDist is < 1.5f and > -1.5f && distance <= playerRange)//prepare for the next attack
        {
            boss.CheckFacingPlayer();
            boss.SetHasAttacked(false);
        }

    }
}
