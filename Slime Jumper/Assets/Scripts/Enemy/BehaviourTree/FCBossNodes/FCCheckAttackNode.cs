
/// <summary>
/// if attacked is set to true -> it will check for when it has attacked
/// if attacked is set to false -> it will check for when it hasn't attacked
/// </summary>
public class FCCheckAttackNode : Node
{
    private FCBossBehaviour boss;
    private bool attacked;
    public FCCheckAttackNode(FCBossBehaviour boss, bool attacked)
    {
        this.boss = boss;
        this.attacked = attacked;
    }

    public override NodeState Evaluate()
    {
        if (attacked) return boss.hasAttacked ? NodeState.SUCCESS : NodeState.FAILURE;
        return !boss.hasAttacked ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
