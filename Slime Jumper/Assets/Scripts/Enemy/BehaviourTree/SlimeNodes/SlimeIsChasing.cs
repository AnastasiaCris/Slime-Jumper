
using UnityEngine;

public class SlimeIsChasing : Node
{
    private SlimeBehaviour slime;
    private Animator anim;

    public SlimeIsChasing(SlimeBehaviour slime)
    {
        this.slime = slime;
        anim = slime.Anim;
    }
    public override NodeState Evaluate()
    {
        if (!slime.chasing)
        {
            anim.SetTrigger("patroll");
        }
        return slime.chasing ? NodeState.FAILURE : NodeState.SUCCESS;
    }
}
