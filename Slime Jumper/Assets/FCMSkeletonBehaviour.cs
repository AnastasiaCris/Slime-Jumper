using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCMSkeletonBehaviour : BaseEnemyBehaviour
{
    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private LayerMask playerLayer;
    
    [field: Header("Behaviour")]
    private bool ready;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;
    public bool canAttack;
    public bool inAttackRange;

    //Animation
    public int attack1AnimHash { get; private set; }
    public int attack2AnimHash { get; private set; }
    public int chasingAnimHash { get; private set; }
    public int resurrectAnimHash { get; private set; }
    public int startAnimHash { get; private set; }
    protected override void Start()
    {
        attack1AnimHash = Animator.StringToHash("attack1");
        attack2AnimHash = Animator.StringToHash("attack2");
        chasingAnimHash = Animator.StringToHash("chasing");
        resurrectAnimHash = Animator.StringToHash("resurrect");
        startAnimHash = Animator.StringToHash("start");
        
        base.Start();
    }

    protected override void Update()
    {
        
        if (State == State.Patrolling && Anim.GetBool(chasingAnimHash))
        {
            Anim.SetBool(chasingAnimHash, false);
        }else if (State == State.Chasing && !Anim.GetBool(chasingAnimHash))
        {
            Anim.SetBool(chasingAnimHash, true);
        }
        if(!ready)return;
        base.Update();
    }
    
    //------------------------------Behaviour Tree---------------------------------

    protected override void ConstructBehaviourTree()
    {
        //ressurected node
        BasicPatrollingNode basicPatrollingNode = new BasicPatrollingNode(this, EnemyScriptableObject, tileLayer);
        FCMSkeletonAttackNode skeletonAttackNode = new FCMSkeletonAttackNode(this, EnemyScriptableObject);
        SeePlayerNode seePlayerNode = new SeePlayerNode(this, playerLayer);
        RangeNode attackRangeNode = new RangeNode(transform, attackRange, true);
        RangeNode chaseRangeNode = new RangeNode(transform, chaseRange, true);
        //FCMSkeletonChaseNode skeletonChaseNode = new FCMSkeletonChaseNode();

        Sequence patrollingSequence = new Sequence(new List<Node> { seePlayerNode, basicPatrollingNode });
        //Sequence chasingSequence = new Sequence(new List<Node>{chaseRangeNode });//chaseRangeNode, skeletonChaseNode
        Sequence attackSequence = new Sequence(new List<Node>{attackRangeNode, skeletonAttackNode});

        topNode = new Selector(new List<Node> {patrollingSequence, attackSequence });

    }
    
    //------------------------------Ready---------------------------------

    public override void StartEnemy()
    {
        Anim.SetTrigger(startAnimHash);
    }
    public override void ResetEnemy()
    {
        base.ResetEnemy();
        Anim.SetTrigger(resurrectAnimHash);
    }
    
    private void ChangeReady(int param)//0 is false, anything else true
    {
        bool par = Convert.ToBoolean(param);
        ready = par;
    }
    
    //------------------------------Attack sequence---------------------------------

    protected override void DealDamage()
    {
        if (inAttackRange)
        {
            player.DamagePlayer(EnemyScriptableObject.damage);
        }
    }
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
