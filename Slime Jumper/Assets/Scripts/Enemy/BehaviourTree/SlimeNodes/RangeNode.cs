
using UnityEngine;

public class RangeNode : Node
{
    private float range;
    private Transform target;
    private Transform enemy;

    public RangeNode (Transform enemy, Transform target, float range)
    {
        this.enemy = enemy;
        this.target = target;
        this.range = range;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector2.Distance(target.position, enemy.transform.position);
        return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
