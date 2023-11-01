using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [Header("Player Elements")]
    Vector2 moveImput;
    Rigidbody2D myRigidbody2D;
    Animator myAnimator;

    CapsuleCollider2D myBodyCollider;

    BoxCollider2D legCollider;

    [SerializeField] int myTimer;



    [Header("Player Movement")]
    [SerializeField] float playerSpeed = 10;
    [SerializeField] float jumpPower = 10;

    [SerializeField] float climbingSpeed = 5f;

    [SerializeField] List<Animation> attackAnimations;


    float startingGravity;

    int attackCounter = 0;
    float attackTimer = 0;


    [SerializeField] int comboDeactivation = 10;
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        legCollider = GetComponent<BoxCollider2D>();


        startingGravity = myRigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        FlipSprite();

        ComboTimer();

    }

    void OnMove(InputValue value)
    {
        moveImput = value.Get<Vector2>();
        myAnimator.SetBool("IsAttacking", false);
    }
    void Move()
    {
        Vector2 playerVelocity = new Vector2(moveImput.x * playerSpeed, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;
        bool playerMovesOnX = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("IsRunning", playerMovesOnX);



    }

    void OnJump(InputValue value)
    {
        //While holding space keep the first frame of animation
        //jump can be charged until the space bar is not released
        // before the player starts to fall, keep 2nd frame
        //when player starts to fall, play 3rd frame
        //check if !doubleJump, do the second jump, while playing double jump animation with the same rules

        if (!legCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (value.isPressed)
        {

            myRigidbody2D.velocity += new Vector2(0f, jumpPower);
        }
    }

    void FlipSprite()
    {

        bool playerMovesOnX = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon; //smart way of saying movement on x > 0

        if (playerMovesOnX)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f); //Using Mathf.Sign set 0 value as positive,
                                                                                          // causing the player to flip to the right
        }
    }
    void OnFire(InputValue value)
    {

        // add + 1 to attack counter
        attackCounter += 1;

        UnityEngine.Debug.Log(attackTimer);

        if (!myAnimator.GetBool("IsAttacking"))
        {

            attackCounter = 0;
            attackTimer = 20;
            myAnimator.SetInteger("AttackCounter", attackCounter);

            myAnimator.SetBool("IsAttacking", true);
            UnityEngine.Debug.Log("Combo has been started");


        }

        if (myAnimator.GetBool("IsAttacking") && attackCounter != 3)
        {
            attackTimer += 10;
            myAnimator.SetInteger("AttackCounter", attackCounter);
            UnityEngine.Debug.Log(attackCounter);


        }

        else if (myAnimator.GetBool("IsAttacking") && attackCounter == 3)
        {
            attackCounter = 0;
            attackTimer += 10;
            UnityEngine.Debug.Log("attack timer is " + attackTimer);

            myAnimator.SetInteger("AttackCounter", attackCounter);


        }



        // if attack counter > 3 reset back to 0
        // if input was not pressed for two seconds set bool to false

    }

    IEnumerator DeactivateCombo()
    {

        yield return new WaitForSecondsRealtime(comboDeactivation);
        myAnimator.SetBool("IsAttacking", false);
        attackCounter = 0;

    }

    void ComboCheck()
    {



        if (myAnimator.GetBool("IsAttacking") && attackTimer < 0)
        {

            StartCoroutine(DeactivateCombo());
            UnityEngine.Debug.Log("Combo has been reset");
        }

    }

    void ComboTimer()
    {
        while (attackTimer > 0)
        {

            attackTimer -= Time.deltaTime;
            UnityEngine.Debug.Log("attack timer is " + attackTimer);
        }

        ComboCheck();
    }

}
