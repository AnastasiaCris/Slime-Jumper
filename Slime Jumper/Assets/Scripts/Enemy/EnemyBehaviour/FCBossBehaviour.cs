using System;
using System.Collections.Generic;
using UnityEngine;

public class FCBossBehaviour : BaseEnemyBehaviour
{
    //Can Enable
    [SerializeField] private List<EnemyType> enemyTypes = new List<EnemyType>();
    [SerializeField] private List<SlimeBehaviour> allEnemies = new List<SlimeBehaviour>();
    [SerializeField] private List<FCSpecialTile> specialTiles = new List<FCSpecialTile>();

    //Behaviour
    [SerializeField] private Stage stage = Stage.Stage1;
    [SerializeField] private float attackRange = 3;
    [SerializeField]private float idleTime = 3;
    public bool InAttackRange { get; private set; }
    public bool hasIdled { get; private set; }
    public bool isIdleInAnim { get; private set; }
    public bool idleFinished { get; private set; }
    public bool canAttack { get; private set; }
    private int direction = 1;
    public bool isAttacking { get; private set; }
    public bool hasAttacked { get; private set; }
    private bool isTeleporting;
    private bool isDying;

    //Animation
    private bool animEnd;
    public int attackAnimHash { get; private set; }
    public int attackingAnimHash { get; private set; }
    public int fakeRecoveryAnimHash { get; private set; }
    public bool canBeDamaged { get; private set; }

    protected override void Start()
    {
        hasIdled = true;
        canBeDamaged = true;
        
        attackingAnimHash = Animator.StringToHash("attacking");
        attackAnimHash = Animator.StringToHash("attack");
        fakeRecoveryAnimHash = Animator.StringToHash("attack2");
        
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        //test
        if (currentHP > maxHP * 0.7f && currentHP <= maxHP) stage = Stage.Stage1;
        else if (currentHP > maxHP * 0.5f && currentHP <= maxHP * 0.7f) stage = Stage.Stage2;
        else if (currentHP > maxHP * 0f && currentHP <= maxHP * 0.5f) stage = Stage.Stage3;
        
    }

    //------------------------------Behaviour Tree---------------------------------

    protected override void ConstructBehaviourTree()
    {
        FCBossStageNode checkStage1Node = new FCBossStageNode(currentHP, maxHP * 0.7f, maxHP); // while above 70% of maxHP and lower then 100%
        FCBossStageNode checkStage2Node = new FCBossStageNode(currentHP, maxHP * 0.5f, maxHP * 0.7f); // while above 50% of maxHP and lower then 70%
        FCBossStageNode checkStage3Node = new FCBossStageNode(currentHP, 0, maxHP * 0.5f); // while above 0% of maxHP and lower then 50%
        
        FCAttackNode stage1AttackNode = new FCAttackNode(this, new List<FCAttackType> { FCAttackType.Attack , FCAttackType.Attack});
        FCAttackNode stage2AttackNode = new FCAttackNode(this, new List<FCAttackType> { FCAttackType.Attack , FCAttackType.Attack, FCAttackType.FakeRecovery, FCAttackType.Attack, FCAttackType.Attack});
        FCAttackNode stage3AttackNode = new FCAttackNode(this, new List<FCAttackType> { FCAttackType.Attack , FCAttackType.FakeRecovery, FCAttackType.Attack, FCAttackType.Attack, FCAttackType.Attack});

        FCCheckAttackNode hasAttackedNode = new FCCheckAttackNode(this, true);
        FCCheckAttackNode hasNotAttackedNode = new FCCheckAttackNode(this, false);
        
        FCTeleportNode teleportNode = new FCTeleportNode(this);
        
        RangeNode isPlayerCloseNode = new RangeNode(transform, attackRange, true);
        RangeNode isPlayerFarNode = new RangeNode(transform, attackRange, false);

        IdleNode idleNode = new IdleNode(this, idleTime, isPlayerCloseNode);
        FCCheckIdleFinishedNode idleNotFinishedNode = new FCCheckIdleFinishedNode(this,false);

        FCActivateTiles activateSomeSpecialTilesNode = new FCActivateTiles(this, false, specialTiles);
        FCActivateTiles activateAllSpecialTilesNode = new FCActivateTiles(this,true, specialTiles);

        FCSummonNode resurrectEnemyNode = new FCSummonNode(this,false, enemyTypes, allEnemies);
        FCSummonNode resurrectAllEnemiesNode = new FCSummonNode(this,true, enemyTypes, allEnemies);
        
        Sequence idleSequence = new Sequence(new List<Node>{hasAttackedNode, idleNotFinishedNode, idleNode});

        Sequence teleportSequence = new Sequence(new List<Node>{isPlayerFarNode, teleportNode});
        
        Sequence stage1AttackSequence = new Sequence(new List<Node>{isPlayerCloseNode,hasNotAttackedNode, stage1AttackNode});
        Sequence stage2AttackSequence = new Sequence(new List<Node>{isPlayerCloseNode,hasNotAttackedNode, stage2AttackNode});
        Sequence stage3AttackSequence = new Sequence(new List<Node>{isPlayerCloseNode,hasNotAttackedNode, stage3AttackNode});

        Sequence stage2ActivationSequence = new Sequence(new List<Node>{activateSomeSpecialTilesNode, resurrectEnemyNode});
        Sequence stage3ActivationSequence = new Sequence(new List<Node>{activateAllSpecialTilesNode, resurrectAllEnemiesNode});
        
        Selector stage1Selector = new Selector(new List<Node> {stage1AttackSequence, idleSequence, teleportSequence});
        Selector stage2Selector = new Selector(new List<Node>{stage2ActivationSequence, stage2AttackSequence, idleSequence, teleportSequence});
        Selector stage3Selector = new Selector(new List<Node>{stage3ActivationSequence, stage3AttackSequence, idleSequence, teleportSequence});

        Sequence stage1Sequence = new Sequence(new List<Node>{checkStage1Node, stage1Selector});
        Sequence stage2Sequence = new Sequence(new List<Node>{checkStage2Node, stage2Selector});
        Sequence stage3Sequence = new Sequence(new List<Node>{checkStage3Node, stage3Selector});
        
        topNode = new Selector(new List<Node> {stage1Sequence, stage2Sequence, stage3Sequence});
    }

    //------------------------------Damage---------------------------------

    public override void TakeDamage(int damage)
    {
        if(!canBeDamaged) return;
        base.TakeDamage(damage);
    }
    
    protected override void DealDamage()
    {
        if (InAttackRange)
        {
            player.DamagePlayer(EnemyScriptableObject.damage);
        }
    }
    
    //plays at the end of death animation
    protected override void EnemyDead()
    {
        //give player points or something
    }
    
    //------------------------------Attack---------------------------------

    public void SetCanAttack(bool can)
    {
        canAttack = can;
    }
    public void SetHasAttacked(bool attacked)
    {
        hasAttacked = attacked;
    }

    public void SetIsAttacking(bool attacking)
    {
        isAttacking = attacking;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            InAttackRange = true;
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Anim.SetBool(attackingAnimHash, false);
            InAttackRange = false;
            canAttack = false;
        }
    }

    private void OnTriggerStay2D(Collider2D col) // just in case
    {
        if (col.gameObject.CompareTag("Player") && !InAttackRange)
        {
            InAttackRange = true;
            canAttack = true;
        }
    }

    //------------------------------Movement---------------------------------

    public void SetLocation(Vector2 location)
    {
        transform.position = location;
        CheckFacingPlayer();
    }
    public void ChangeDirection()
    {
        transform.Rotate(Vector3.up, 180f);
        direction *= -1;
    }

    public void CheckFacingPlayer()
    {
        int dirXToPlayer = (int)Mathf.Sign(playerTransform.transform.position.x - transform.position.x);
        if (dirXToPlayer != direction)
        {
            ChangeDirection();
        }
    }

    //------------------------------Animation---------------------------------

    /// <summary>
    /// sets the animEnd variable to a specific state
    /// </summary>
    /// <param name="end"></param>
    public void SetAnimState(bool end)
    {
        animEnd = end;
    }
    //played in animation
    private void SetAnimStateTrue()
    {
        animEnd = true;
    }
    public bool GetAnimState()
    {
        return animEnd;
    }

    //played in animation
    private void SetIdleTrue()
    {
        isIdleInAnim = true;
    }
    public void SetHasIdled(bool idle)
    {
        hasIdled = idle;
    }

    public void SetIdleFinished(bool finished)
    {
        idleFinished = finished;
    }

    public void SetCanBeDamaged(bool canBe)
    {
        canBeDamaged = canBe;
    }
}
public enum FCAttackType{
    Attack, FakeRecovery
}

public enum Stage
{
    Stage1, Stage2, Stage3
}
