
public class FCCheckStageActivation : Node
{
    private FCBossBehaviour boss;
    private int stageNr;

    public FCCheckStageActivation(FCBossBehaviour boss, int stageNr)
    {
        this.boss = boss;
        this.stageNr = stageNr;
    }

    public override NodeState Evaluate()
    {
        switch (stageNr)
        {
            case 2:
                return boss.stage2Activated ? NodeState.SUCCESS : NodeState.FAILURE;
                break;
            case 3:
                return boss.stage3Activated ? NodeState.SUCCESS : NodeState.FAILURE;
                break;
        }

        return NodeState.FAILURE;
    }
}
