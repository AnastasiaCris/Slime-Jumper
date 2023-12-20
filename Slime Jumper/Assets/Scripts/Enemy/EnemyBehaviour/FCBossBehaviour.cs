using System.Collections.Generic;
using UnityEngine;

public class FCBossBehaviour : EnemyBehaviour
{
    //Can effect
    [SerializeField] private List<EnemyType> enemyTypes = new List<EnemyType>();
    [SerializeField] private List<SlimeBehaviour> allEnemies = new List<SlimeBehaviour>();
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();

    //Behaviour
    public bool canAttack;
    private bool isAttacking;
    private bool isTeleporting;
    private bool isDying;
    private float waitBeforeAction;
    private bool inAttackRange;
    
    //Animation
    private bool animEnd;
    private int attackAnimHash;
    private int attackingAnimHash;
    private int fakeRecoveryAnimHash;
    
    protected override void Start()
    {
        base.Start();
        
        attackingAnimHash = Animator.StringToHash("attacking");
        attackAnimHash = Animator.StringToHash("attack");
        fakeRecoveryAnimHash = Animator.StringToHash("attack2");
        
    }
    
    //------------------------------Behaviour Tree---------------------------------

    protected override void ConstructBehaviourTree()
    {
        FCBossStageNode checkStage1Node = new FCBossStageNode(currentHP, maxHP * 0.7f); // while above 70% of maxHP
        FCBossStageNode checkStage2Node = new FCBossStageNode(currentHP, maxHP * 0.5f); // while above 50% of maxHP
        FCBossStageNode checkStage3Node = new FCBossStageNode(currentHP, 0); // while above 0% of maxHP
        
        FCAttackNode stage1AttackNode = new FCAttackNode(this, new List<int> { attackAnimHash , attackAnimHash});
        FCAttackNode stage2AttackNode = new FCAttackNode(this, new List<int> { attackAnimHash , attackAnimHash, fakeRecoveryAnimHash, attackAnimHash});
        FCAttackNode stage3AttackNode = new FCAttackNode(this, new List<int> { attackAnimHash , fakeRecoveryAnimHash, attackAnimHash, attackAnimHash});
        
        FCTeleportNode teleportNode = new FCTeleportNode();
        
        FCCheckDistanceNode isPlayerCloseNode = new FCCheckDistanceNode();
        FCCheckDistanceNode isPlayerFarNode = new FCCheckDistanceNode();

        FCActivateTiles activateSomeSpecialTilesNode = new FCActivateTiles(this, false);
        FCActivateTiles activateAllSpecialTilesNode = new FCActivateTiles(this,true);

        FCSummonNode resurrectEnemyNode = new FCSummonNode(this,false, enemyTypes, allEnemies);
        FCSummonNode resurrectAllEnemiesNode = new FCSummonNode(this,true, enemyTypes, allEnemies);
        
        Sequence teleportSequence = new Sequence(new List<Node>{isPlayerFarNode, teleportNode});
        
        Sequence stage1AttackSequence = new Sequence(new List<Node>{isPlayerCloseNode, stage1AttackNode});
        Sequence stage2AttackSequence = new Sequence(new List<Node>{isPlayerCloseNode, stage2AttackNode});
        Sequence stage3AttackSequence = new Sequence(new List<Node>{isPlayerCloseNode, stage3AttackNode});

        Sequence stage2ActivationSequence = new Sequence(new List<Node>(){activateSomeSpecialTilesNode, resurrectEnemyNode});
        Sequence stage3ActivationSequence = new Sequence(new List<Node>(){activateAllSpecialTilesNode, resurrectAllEnemiesNode});
        
        Selector stage1Selector = new Selector(new List<Node>() {stage1AttackSequence, teleportSequence});
        Selector stage2Selector = new Selector(new List<Node>() {stage2ActivationSequence, stage2AttackSequence, teleportSequence});
        Selector stage3Selector = new Selector(new List<Node>() {stage3ActivationSequence, stage3AttackSequence, teleportSequence});

        Sequence stage1Sequence = new Sequence(new List<Node>() {checkStage1Node, stage1Selector});
        Sequence stage2Sequence = new Sequence(new List<Node>() {checkStage2Node, stage2Selector});
        Sequence stage3Sequence = new Sequence(new List<Node>() {checkStage3Node, stage3Selector});
        
        topNode = new Selector(new List<Node> {stage1Sequence, stage2Sequence, stage3Sequence});
    }

    //------------------------------Damage---------------------------------
    
    protected override void DealDamage()
    {
        if (inAttackRange)
        {
            player.DamagePlayer(EnemyScriptableObject.damage);
        }
    }
    
    protected override void EnemyDying()
    {
        //give player points or something
        base.EnemyDying();
    }
    
    //------------------------------Attack---------------------------------
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
            Anim.SetBool(attackingAnimHash, false);
            inAttackRange = false;
            canAttack = false;
        }
    }

    //------------------------------Animation---------------------------------

    public void SetAnimState(bool end)
    {
        animEnd = end;
    }
    public void SetAnimStateTrue()
    {
        animEnd = true;
    }
    public bool GetAnimState()
    {
        return animEnd;
    }
}
public enum FCAttackType{
    Attack, Recovery, FakeRecovery, SpecialAttack
}
