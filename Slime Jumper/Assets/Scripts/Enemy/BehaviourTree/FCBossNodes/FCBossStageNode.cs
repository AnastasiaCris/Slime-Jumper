public class FCBossStageNode : Node
{
    private int currentHP;
    private float hpThreshHold;
    
    public FCBossStageNode(int currentHP, float hpThreshHold)
    {
        this.currentHP = currentHP;
        this.hpThreshHold = hpThreshHold;
    }

    public override NodeState Evaluate()
    {
        return currentHP > hpThreshHold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
