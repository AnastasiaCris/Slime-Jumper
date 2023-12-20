using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : EnemyBehaviour
{
    [field: Header("Properties")]
    [SerializeField] private Transform groundPos;

    [field: Header("Behaviour")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;
    [HideInInspector] public int direction = -1;
    [HideInInspector] public bool chasing;
    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool inAttackRange;
    [HideInInspector] public bool jumping;
    
    [Header("Behaviour Tree")]
    private Sequence patrollingSequence;
    

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        if(patrollingSequence!= null && patrollingSequence.NodeState == NodeState.FAILURE) chasing = false;
        if (jumping) jumping = false;
    }
    
    //------------------------------Behaviour Tree---------------------------------

    protected override void ConstructBehaviourTree()
    {
        SlimePatrollingNode slimePatrollingNode = new SlimePatrollingNode(this, EnemyScriptableObject);
        SlimeChaseNode slimeChaseNode = new SlimeChaseNode(playerTransform, this, EnemyScriptableObject, groundPos);
        SlimeAttackNode slimeAttackNode = new SlimeAttackNode(this, EnemyScriptableObject);
        SlimeIsChasing slimeIsChasingNode = new SlimeIsChasing(this);
        SeePlayerNode seePlayerNode = new SeePlayerNode(this);
        RangeNode chaseRangeNode = new RangeNode(transform, playerTransform, chaseRange);
        RangeNode attackRangeNode = new RangeNode(transform, playerTransform, attackRange);

        patrollingSequence = new Sequence(new List<Node> { slimeIsChasingNode, seePlayerNode, slimePatrollingNode });
        Sequence chasingSequence = new Sequence(new List<Node>{chaseRangeNode, slimeChaseNode});
        Sequence attackSequence = new Sequence(new List<Node>{attackRangeNode, slimeAttackNode});

        topNode = new Selector(new List<Node> {patrollingSequence, chasingSequence, attackSequence });

        if (chasing && chaseRangeNode.NodeState == NodeState.FAILURE) chasing = false;
    }
    
    //------------------------------Damage---------------------------------

    protected override IEnumerator Damaged()
    {
        //play damaged animation
        GetComponent<SpriteRenderer>().color = Color.red;//debug
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;//debug
    }

    protected override void EnemyDying()
    {
        //give player points or something
        base.EnemyDying();
    }

    /// <summary>
    /// Called in the enemy attack animation
    /// </summary>
    protected override void DealDamage()
    {
        if (inAttackRange)
        {
            player.DamagePlayer(EnemyScriptableObject.damage);
        }
    }
    //------------------------------Movement---------------------------------
    
    /// <summary>
    /// Checks if the current tile the enemy is on has ended
    /// </summary>
    /// <returns></returns>
    public bool TileFinished()
    {
        Vector2 boxcastOrigin = direction == 1 ? new Vector2(transform.position.x + 1, transform.position.y - 0.5f) : new Vector2(transform.position.x - 1, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.BoxCast(boxcastOrigin, new Vector2(1, 0.5f), 0f, Vector2.right * direction, 0f, 1 << 6);
        
        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }
    //------------------------------Attack sequence---------------------------------

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inAttackRange = true;
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Anim.SetBool("attacking", false);
            inAttackRange = false;
            canAttack = false;
        }
    }
    
}


