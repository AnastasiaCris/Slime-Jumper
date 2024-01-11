using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCCheckDistanceNode : Node
{
    private Player target;
    private FCBossBehaviour boss;
    public FCCheckDistanceNode()
    {
        target = GameProperties.playerScript;
    }

    public override NodeState Evaluate()
    {
        throw new System.NotImplementedException();
    }
}
