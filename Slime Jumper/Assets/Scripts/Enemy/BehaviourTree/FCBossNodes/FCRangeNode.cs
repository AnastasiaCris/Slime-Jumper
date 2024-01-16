using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCRangeNode : Node
{
    private float range;
    private Transform target;
    private Transform enemy;
    private bool isInRange;

    public FCRangeNode(Transform enemy, float range, bool isInRange)
    {
        this.enemy = enemy;
        this.range = range;
        this.isInRange = isInRange;
        target = GameProperties.playerLoc;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector2.Distance(target.position, enemy.transform.position);
        float yDist = target.position.y - enemy.transform.position.y;
        if (yDist is > 1.5f or < -1.5f) return isInRange? NodeState.FAILURE: NodeState.SUCCESS;//if target is much higher or much lower then enemy
        if (isInRange) return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        return distance > range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
