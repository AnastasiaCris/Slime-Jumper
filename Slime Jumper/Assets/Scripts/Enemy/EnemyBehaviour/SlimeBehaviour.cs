using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : BaseEnemyBehaviour
{
    [field: Header("Properties")]
    [SerializeField] private Transform groundPos;

    [field: Header("Behaviour")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private LayerMask playerLayer;
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

    protected override void Update()
    {
        base.Update();
        if(!chasing)return;
        
        float distance = Vector2.Distance(playerTransform.position, transform.position);
        if (distance > chaseRange) chasing = false;
    }

    //------------------------------Behaviour Tree---------------------------------

    protected override void ConstructBehaviourTree()
    {
        BasicPatrollingNode basicPatrollingNode = new BasicPatrollingNode(this, EnemyScriptableObject, tileLayer);
        SlimeChaseNode slimeChaseNode = new SlimeChaseNode(playerTransform, this, EnemyScriptableObject, groundPos);
        SlimeAttackNode slimeAttackNode = new SlimeAttackNode(this, EnemyScriptableObject);
        SlimeIsChasing slimeIsChasingNode = new SlimeIsChasing(this);
        SeePlayerNode seePlayerNode = new SeePlayerNode(this, playerLayer);
        RangeNode attackRangeNode = new RangeNode(transform, attackRange, true);
        RangeNode chaseRangeNode = new RangeNode(transform, chaseRange, true);

        patrollingSequence = new Sequence(new List<Node> { slimeIsChasingNode, seePlayerNode, basicPatrollingNode });
        Sequence chasingSequence = new Sequence(new List<Node>{chaseRangeNode, slimeChaseNode});
        Sequence attackSequence = new Sequence(new List<Node>{attackRangeNode, slimeAttackNode});

        topNode = new Selector(new List<Node> {patrollingSequence, chasingSequence, attackSequence });

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


