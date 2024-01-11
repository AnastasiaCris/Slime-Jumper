public class FCBossStageNode : Node
{
    private int currentHP;
    private float hpThreshHoldHigher;
    private float hpThreshHoldLower;
    
    public FCBossStageNode(int currentHP, float hpThreshHoldHigher, float hpThreshHoldLower)
    {
        this.currentHP = currentHP;
        this.hpThreshHoldHigher = hpThreshHoldHigher;
        this.hpThreshHoldLower = hpThreshHoldLower;
    }

    public override NodeState Evaluate()
    {
        if (currentHP > hpThreshHoldHigher && currentHP <= hpThreshHoldLower) return NodeState.SUCCESS;
        return NodeState.FAILURE;
    }
}
