using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    [Header("Player Elements")] 
    private PlayerInput controls;
    private InputAction movementControl;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Player Movement")][Space]
    [SerializeField] private float originalPSpeed = 5;
    [SerializeField] private float originalJForce = 14;
    [SerializeField] private Transform groundCheck;
    public bool canControl = true;
    private float playerSpeed = 5;
    private float jumpForce = 14;
    private Vector2 moveDir;
    private float direction = 1;
    private int jumpCounter = 1;
    public bool IsGrounded { get; private set; }
    private bool isJumping;
    private bool preparingJump;
    private bool slowed;

    [Header("Player Properties")] [Space]
    [SerializeField] private float maxHP = 10;
    [SerializeField] public int damage = 5;
    [SerializeField] private float currentHP;
    private bool dead;

    [Header("Attack")] [Space]
    [SerializeField] private int deactivateComboInSec = 2;
    [SerializeField] private float attackForce = 40;
    private Vector2 attackVelocity;
    private WeaponBehaviour weaponBehaviour;
    private int attackCounter;
    private float attackTimer;

    [Header("Extra")]
    private float screenWidth;


    private void OnEnable()
    {
        controls = GetComponent<PlayerInput>();
        movementControl = controls.actions.FindAction("Move");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weaponBehaviour = gameObject.GetComponentInChildren<WeaponBehaviour>();

        screenWidth = GameProperties.screenWidth;

        currentHP = maxHP;
        playerSpeed = originalPSpeed;
        jumpForce = originalJForce;
    }

    void Update()
    {
        if(dead)
            return;
        
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, 1 << 7);
        ReachedEndOfScreen();
        
        if(!canControl)
            return;
        
        Move();
        if ((IsGrounded || !IsGrounded && jumpCounter > 0) && Input.GetButtonDown("Jump")) StartCoroutine(Jump());
        ChangeSpriteDirection();
    }
    
    //------------------------------Movement------------------------------

    void OnMove()
    {
        if (preparingJump && IsGrounded) // if it's at the first frame of jumping then stop jumping
        {
            anim.SetBool("IsJumping", false);
            if(!slowed)jumpCounter = 1;
            preparingJump = false;
        }
    }

    void Move()
    {
        moveDir = movementControl.ReadValue<Vector2>();
        Vector2 playerVelocity = new Vector2(moveDir.x * playerSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        bool playerMovesOnX = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        anim.SetBool("IsRunning", playerMovesOnX);
    }

    private IEnumerator Jump()
    {
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        preparingJump = true;
        anim.SetBool("IsJumping", true);
        anim.SetInteger("JumpCount", 0); //set the first frame of the jump animation
        if (!IsGrounded && isJumping)
                jumpCounter--;

        yield return new WaitUntil(() => Input.GetButtonUp("Jump"));
        if (preparingJump)
        {
            anim.SetInteger("JumpCount", 1); //set the second frame of the jump animation
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //jump
            preparingJump = false;
            isJumping = true;

            yield return new WaitUntil(() => rb.velocity.y < 0); //wait until falling
            anim.SetInteger("JumpCount", 2); //set the third frame of the jump animation

            yield return new WaitUntil(() => IsGrounded); //wait until grounded
            anim.SetInteger("JumpCount", 0); //set the first frame of the jump animation
            anim.SetBool("IsJumping", false);
            isJumping = false;
            jumpCounter = 1;

        }
    }

    private void ReachedEndOfScreen()
    {
        //Change player pos if he went through one side of the screen
        Vector3 newPosition = transform.position;

        if (transform.position.x > screenWidth / 2)
        {
            newPosition.x = -screenWidth / 2;
        }
        else if (transform.position.x < -screenWidth / 2)
        {
            newPosition.x = screenWidth / 2;
        }

        transform.position = newPosition;
    }

    private void ChangeSpriteDirection()
    {
        if(moveDir.x == 0)
            return;
        if (direction != moveDir.x)
        {
            transform.Rotate(Vector2.up, 180);
            direction = moveDir.x;
        }
    }
    
    //------------------------------Attacking------------------------------
    void OnFire(InputValue value)
    {
        if(dead || isJumping || !canControl)
            return;
        
        if (!anim.GetBool("IsAttacking") || attackCounter > 2)
        {
            attackCounter = 0;
            anim.SetInteger("AttackCounter", attackCounter);
            anim.SetBool("IsAttacking", true);
            anim.SetTrigger("Attack");
        }else if (anim.GetBool("IsAttacking"))
        {
            anim.SetTrigger("Attack");

            if(attackCounter == 1) anim.SetInteger("AttackCounter", attackCounter);
            else if (attackCounter == 2) anim.SetInteger("AttackCounter", attackCounter);
        }

        attackCounter ++;// add + 1 to attack counter
        StopCoroutine(ComboTimer());
        StartCoroutine(ComboTimer());
        
    }
    IEnumerator ComboTimer()
    {
        attackTimer = deactivateComboInSec;
        while (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            yield return null;
        }

        if (attackTimer <= 0 && anim.GetBool("IsAttacking")) //end attack sequence
        {
            anim.SetBool("IsAttacking", false);
            attackCounter = 0;
        }
    }

    /// <summary>
    /// Called during the attack animation
    /// </summary>
    private void DealDamage()
    {
        if (weaponBehaviour.enemiesInAttackRange.Count > 0 )
        {
            foreach (var enemyScript in weaponBehaviour.enemiesInAttackRange)
            {
                enemyScript.TakeDamage(damage);
                
                Debug.Log("Enemy damaged");
            }

        }

        if (weaponBehaviour.objectsInAttackRange.Count > 0)
        {
            foreach (var objectScript in weaponBehaviour.objectsInAttackRange)
            {
                objectScript.ObjectCollapse();
                
                Debug.Log("Object smashed");
            }
            
        }
    }

    //------------------------------Effects------------------------------
    /// <summary>
    /// Slows the players movement speed and jump force
    /// </summary>
    /// <param name="slowingAmount"></param>
    /// <param name="slowJumpAmount"></param>
    public void SlowDown(float slowingAmount, float slowJumpAmount)
    {
        slowed = true;
        playerSpeed *= slowingAmount;
        jumpForce *= slowJumpAmount;
        jumpCounter = 0;
    }

    /// <summary>
    /// Resets player speed and jump force
    /// </summary>
    public void UnSlow()
    {
        slowed = false;
        playerSpeed = originalPSpeed;
        jumpForce = originalJForce;
    }
    //------------------------------Death------------------------------

    public void DamagePlayer(float dmgAmount)
    {
        if (currentHP > 0)
        {
            currentHP -= dmgAmount;
            StartCoroutine(Damaged());
        }
        if (currentHP <= 0)
        {
            PlayerDeath();
        }
    }

    IEnumerator Damaged()
    {
        GetComponent<SpriteRenderer>().color = Color.red;//debug
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;//debug
    }

    public void PlayerDeath()
    {
        dead = true;
        anim.SetTrigger("isDying");   
    }
    
    public void ResetScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheck.position, 0.3f);
    }
}
