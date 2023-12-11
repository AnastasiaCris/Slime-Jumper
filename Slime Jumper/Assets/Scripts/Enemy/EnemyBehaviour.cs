using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Properties")] 
    [SerializeField] private Enemy enemy;
    public Enemy MyEnemy { get { return enemy; } private set { enemy = value; } }
    [SerializeField] private Transform groundPos;
    public int maxHP;
    public int currentHP;
    [HideInInspector]public Animator anim;
    
    [Header("Player properties")]
    private Transform playerTransform;
    private Player player;
    
    [Header("Behaviour")]
    public State state = State.Nothing;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;
    [HideInInspector] public int direction = -1;
    [HideInInspector] public bool chasing;
    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool inAttackRange;
    [SerializeField] public bool JUMPING; //debug
    
    [Header("Behaviour Tree")]
    private Node topNode;
    public Sequence patrollingSequence;
    public Sequence chasingSequence;
    public Sequence attackSequence;

    void Start()
    {
        maxHP = enemy.maxHP;
        currentHP = maxHP;
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        player = playerTransform.GetComponent<Player>();
        ConstructBehaviourTree();
    }

    void Update()
    {
        topNode.Evaluate();
    }

    private void ConstructBehaviourTree()
    {
        PatrollingNode patrollingNode = new PatrollingNode(this, enemy);
        ChaseNode chaseNode = new ChaseNode(playerTransform, this, enemy, groundPos);
        AttackNode attackNode = new AttackNode(this, enemy);
        StartedChasing startChasingNode = new StartedChasing(this);
        SeePlayerNode seePlayerNode = new SeePlayerNode(this);
        RangeNode chaseRangeNode = new RangeNode(this, playerTransform, chaseRange);
        RangeNode attackRangeNode = new RangeNode(this, playerTransform, attackRange);

        patrollingSequence = new Sequence(new List<Node> { startChasingNode, seePlayerNode, patrollingNode });
        chasingSequence = new Sequence(new List<Node>{chaseRangeNode, chaseNode});
        attackSequence = new Sequence(new List<Node>{attackRangeNode, attackNode});

        topNode = new Selector(new List<Node> {patrollingSequence, chasingSequence, attackSequence });
    }
    
    //------------------------------Damage---------------------------------

    public void TakeDamage(int damage)
    {
        if (currentHP > 0)
        {
           currentHP -= damage;
            StartCoroutine(Damaged()); 
        }
        
        if (currentHP <= 0)
        {
            currentHP = 0;
            anim.SetTrigger("dead");
        }
    }
    
    IEnumerator Damaged()
    {
        GetComponent<SpriteRenderer>().color = Color.red;//debug
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;//debug
    }
    
    /// <summary>
    /// Plays in the death animation of the enemy
    /// </summary>
    public void EnemyDead()
    {
        //give player points or something
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called in the enemy attack animation
    /// </summary>
    public void DealDamage()
    {
        if (inAttackRange)
        {
            //damage player
            player.DamagePlayer(enemy.damage);
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
            anim.SetBool("attacking", false);
            inAttackRange = false;
            canAttack = false;
        }
    }
    
}

public enum State{
Nothing, Patrolling, Chasing, Attacking
}
