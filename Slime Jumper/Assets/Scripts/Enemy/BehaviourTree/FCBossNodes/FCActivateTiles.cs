using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCActivateTiles : Node
{
    private FCBossBehaviour boss;
    private bool activateAll;

    public FCActivateTiles(FCBossBehaviour boss, bool activateAll)
    {
        this.boss = boss;
        this.activateAll = activateAll;
    }

    public override NodeState Evaluate()
    {
        throw new System.NotImplementedException();
    }
}
