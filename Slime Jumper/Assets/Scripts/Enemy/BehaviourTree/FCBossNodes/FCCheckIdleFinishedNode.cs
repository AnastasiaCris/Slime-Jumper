using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCCheckIdleFinishedNode : Node
{
    private FCBossBehaviour boss;
    private bool finish;
    public FCCheckIdleFinishedNode(FCBossBehaviour boss, bool finish)
    {
        this.boss = boss;
        this.finish = finish;
    }

    public override NodeState Evaluate()
    {
        if(finish) return boss.idleFinished ? NodeState.SUCCESS : NodeState.FAILURE;
        return !boss.idleFinished ? NodeState.SUCCESS : NodeState.FAILURE;
        
    }
}
