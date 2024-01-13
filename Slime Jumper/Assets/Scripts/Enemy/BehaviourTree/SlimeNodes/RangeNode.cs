
using UnityEngine;

/// <summary>
/// (if isInRange)Is a success when the distance between target and enemy is smaller then the range
/// (if !isInRange)Is a success when the distance between target and enemy is bigger then the range
/// </summary>
public class RangeNode : Node
{
    private float range;
    private Transform target;
    private Transform enemy;
    private bool isInRange;

    public RangeNode (Transform enemy, float range, bool isInRange)
    {
        this.enemy = enemy;
        this.range = range;
        this.isInRange = isInRange;
        target = GameProperties.playerLoc;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector2.Distance(target.position, enemy.transform.position);
        if(isInRange) return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        return distance > range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
