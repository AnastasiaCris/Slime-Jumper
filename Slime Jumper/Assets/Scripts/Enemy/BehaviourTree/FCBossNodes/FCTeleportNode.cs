using System.Collections;
using UnityEngine;

public class FCTeleportNode : Node
{
    private Transform target;
    private FCBossBehaviour boss;

    //Timer
    private float timerAmount = 3;
    private bool timerFinished;
    private bool timerStarted;
    
    //Animation
    private Animator anim;
    private int teleportAnimHash;
    private int showTeleportAnimHash;
    private WaitUntil waitUntilGrounded;
    private WaitUntil waitUntilIdle;
    private WaitForSeconds waitSec = new WaitForSeconds(2);
    
    private bool startedTeleport;
    private bool finishedTeleporting;
    public FCTeleportNode(FCBossBehaviour boss)
    {
        target = GameProperties.playerLoc;
        this.boss = boss;
        anim = boss.Anim;

        waitUntilGrounded = new WaitUntil(() => GameProperties.playerScript.IsGrounded && boss.animEnd);
        waitUntilIdle = new WaitUntil(() => boss.isIdleInAnim && !boss.isAttacking);
        teleportAnimHash = Animator.StringToHash("teleport");
        showTeleportAnimHash = Animator.StringToHash("showTeleportLoc");
    }

    public override NodeState Evaluate()
    {
        boss.ChangeState(State.Teleporting);

        if (!boss.hasAttacked && !timerStarted && boss.hasIdled)
        {
            timerStarted = true;
            boss.StopCoroutine(StartTimer());
            boss.StartCoroutine(StartTimer());
        }
        if (!startedTeleport && boss.hasIdled && (boss.hasAttacked || timerFinished))
        {
            boss.StartCoroutine(Teleport());
            startedTeleport = true;
            timerFinished = false;
        }

        return finishedTeleporting ? NodeState.FAILURE : NodeState.SUCCESS;
    }

    /// <summary>
    /// turn on animation for teleport start
    /// wait a couple sec
    /// show the teleportation loc anim
    /// end of teleportation anim
    /// </summary>
    /// <returns></returns>
    private IEnumerator Teleport()
    {
        yield return waitUntilIdle;
        anim.SetTrigger(teleportAnimHash);
        boss.SetCanAttack(false);
        boss.SetCanBeDamaged(false);
        boss.SetAnimState(false);
        yield return waitUntilGrounded;
        
        float locX = SetRandomXAroundTarget(target.position.x, 3);
        Vector2 locationPos = new Vector2(locX, target.position.y);
        boss.SetLocation(locationPos);
        anim.SetBool(showTeleportAnimHash, true);
        
        boss.SetAnimState(false);
        yield return waitSec;
        anim.SetBool(showTeleportAnimHash, false);
        
        boss.SetAnimState(false);
        yield return null;
        boss.SetCanBeDamaged(true);
        boss.SetCanAttack(true);
        finishedTeleporting = true;
        startedTeleport = false;
        
        boss.CheckFacingPlayer();
        boss.SetHasAttacked(false); //reset the hasAttacked bool
    }

    /// <summary>
    /// Randomly teleport on either the left or right side of the target
    /// </summary>
    private float SetRandomXAroundTarget(float targetX, float maxRange)
    {
        float randomX = 0;
        while(randomX is > -1 and < 1) // make sure it never is on top of the target
            randomX = Random.Range(targetX - maxRange, targetX + maxRange);
        return randomX;
    }
    
    //------------------------Timer-------------------------
    IEnumerator StartTimer()
    {
        timerFinished = false;
        yield return new WaitForSeconds(timerAmount);
        timerFinished = true;
    }
}
