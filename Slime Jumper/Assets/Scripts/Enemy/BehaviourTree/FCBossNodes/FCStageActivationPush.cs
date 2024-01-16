using System.Collections;
using UnityEngine;

public class FCStageActivationPush : Node
{
    private FCBossBehaviour boss;
    private Animator anim;
    private float range;
    private float pushForce;

    private bool pushed;

    public FCStageActivationPush(FCBossBehaviour boss, float range, float pushForce)
    {
        this.boss = boss;
        this.range = range;
        this.pushForce = pushForce;
        anim = boss.Anim;
    }

    public override NodeState Evaluate()
    {
        if (!pushed)
        {
            boss.SetHasIdled(true);
            boss.StartCoroutine(PushAway());
            pushed = true;
        }

        return pushed ? NodeState.FAILURE : NodeState.RUNNING;
    }

    private IEnumerator PushAway()
    {
        anim.SetTrigger(boss.stageActivationAnimHash);
        yield return new WaitForSeconds(0.1f);
        float distance = Vector3.Distance(boss.transform.position, GameProperties.playerLoc.position);
        if (distance < range)
        {
            Vector3 direction =  GameProperties.playerLoc.position - boss.transform.position;
            
            direction.Normalize();

            GameProperties.playerScript.canControl = false;
            // Apply force to push the object away
            GameProperties.playerRb.velocity = direction * pushForce;
            
            yield return new WaitForSeconds(0.2f);
            GameProperties.playerScript.canControl = true;
            
        }
    }
}
