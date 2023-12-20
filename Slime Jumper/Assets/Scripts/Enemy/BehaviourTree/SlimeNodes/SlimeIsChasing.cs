
using UnityEngine;

public class SlimeIsChasing : Node
{
    private SlimeBehaviour _slime;
    private Animator anim;

    public SlimeIsChasing(SlimeBehaviour slime)
    {
        this._slime = slime;
        anim = slime.Anim;
    }
    public override NodeState Evaluate()
    {
        if (!_slime.chasing)
        {
            anim.SetTrigger("patroll");
        }
        return _slime.chasing ? NodeState.FAILURE : NodeState.SUCCESS;
    }
}
