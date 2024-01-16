public class FCBossStageNode : Node
{
    private FCBossBehaviour boss;
    private float hpThreshHoldHigher;
    private float hpThreshHoldLower;
    
    public FCBossStageNode(FCBossBehaviour boss, float hpThreshHoldHigher, float hpThreshHoldLower)
    {
        this.boss = boss;
        this.hpThreshHoldHigher = hpThreshHoldHigher;
        this.hpThreshHoldLower = hpThreshHoldLower;
    }

    public override NodeState Evaluate()
    {
        if (boss.currentHP > hpThreshHoldHigher && boss.currentHP <= hpThreshHoldLower) return NodeState.SUCCESS;
        return NodeState.FAILURE;
    }
}
