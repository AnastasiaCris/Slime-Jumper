
using UnityEngine;

public class RangeNode : Node
{
    private float range;
    private Transform target;
    private EnemyBehaviour enemy;

    public RangeNode (EnemyBehaviour enemy, Transform target, float range)
    {
        this.enemy = enemy;
        this.target = target;
        this.range = range;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector2.Distance(target.position, enemy.transform.position);
        enemy.chasing = distance <= range;
        return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        
    }
}
