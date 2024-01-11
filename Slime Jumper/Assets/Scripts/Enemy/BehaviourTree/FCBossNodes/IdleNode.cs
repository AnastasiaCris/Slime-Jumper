using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Node
{
    private FCBossBehaviour boss;
    private float timeToIdle;
    private bool idleStarted;
    private Node playerClose;
    public IdleNode(FCBossBehaviour boss, float timeToIdle, Node playerClose)
    {
        this.boss = boss;
        this.timeToIdle = timeToIdle;
        this.playerClose = playerClose;
    }

    public override NodeState Evaluate()
    {
        boss.ChangeState(State.Idle);
        if (!boss.idleFinished && !idleStarted)
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
        boss.CheckFacingPlayer();
        idleStarted = false;
        
        if(playerClose.NodeState == NodeState.SUCCESS) //prepare for the next attack
            boss.SetHasAttacked(false);
    }
}
