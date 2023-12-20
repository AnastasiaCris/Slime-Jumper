using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EnemyBehaviour : MonoBehaviour
{
    //properties
    [field: SerializeField] public EnemyScriptableObject EnemyScriptableObject { get; private set; }
    protected int maxHP;
    protected int currentHP;
    
    //animation
    [field: SerializeField]public Animator Anim { get; private set; }
    protected int DeadAnimHash = Animator.StringToHash("dead");

    //player properties
    protected Transform playerTransform;
    protected Player player;

    //Behaviour 
    [field:SerializeField]public State State { get; private set; } = State.Nothing;

    protected Node topNode;

    protected virtual void Start()
    {
        maxHP = EnemyScriptableObject.maxHP;
        currentHP = maxHP;
        playerTransform = GameProperties.playerLoc;
        player = GameProperties.playerScript;
        ConstructBehaviourTree();
    }

    protected virtual void Update()
    {
        topNode.Evaluate();
    }

    public virtual void ResetEnemy()
    {
        currentHP = maxHP;
    }
    
    //------------------------------Behaviour Tree---------------------------------

    public void ChangeState(State stateChange)
    {
        State = stateChange;
    }

    protected virtual void ConstructBehaviourTree()
    {
        
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
            EnemyDying();
        }
    }
    
    protected virtual IEnumerator Damaged()
    {
        GetComponent<SpriteRenderer>().color = Color.red;//debug
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;//debug
    }

    protected virtual void EnemyDying()
    {
        Anim.SetTrigger(DeadAnimHash);
    }
    
    /// <summary>
    /// Plays in the death animation of the enemy
    /// </summary>
    public void EnemyDeadInAnimation()
    {
        gameObject.SetActive(false);
    }

    protected virtual void DealDamage()
    {
        player.DamagePlayer(EnemyScriptableObject.damage);
    }
}

public enum State{
    Nothing, Patrolling, Chasing, Attacking
}

public enum EnemyType{
    Boss, SmallSlime, Skeleton, Archer, Mage
}
