using System.Collections;
using UnityEngine;

public class ChaseNode : Node
{
    private Transform target;
    private EnemyBehaviour enemy;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform groundPos;
    private float jumpForce;
    private float moveSpeed;
    private bool isGrounded;
    private bool canJump = true;
    private bool jumping;

    public ChaseNode(Transform target, EnemyBehaviour enemy, Enemy enemyStats,Transform groundPos)
    {
        this.target = target;
        this.enemy = enemy;
        this.groundPos = groundPos;
        jumpForce = enemyStats.jumpHeight;
        moveSpeed = enemyStats.speed;
        anim = enemy.anim;
        rb = enemy.GetComponent<Rigidbody2D>();
    }
    
    public override NodeState Evaluate()
    {
        float distance = Vector2.Distance(target.position, enemy.transform.position);
        
        if (distance > 1.4f)//so it doesn't overlap with the player
        {
            FollowPlayer();
            
            anim.SetTrigger("chasing");
            enemy.state = State.Chasing;

            enemy.chasing = true;
            return NodeState.RUNNING;
        }else
        {
            //stop the enemy
            return NodeState.FAILURE;
        }
    }
    
    /// <summary>
    /// Moves towards player direction, if enemy detected that the tile he is on has ended => find the closest tile and jump to it
    /// </summary>
    private void FollowPlayer()
    {
        isGrounded = Physics2D.Raycast(groundPos.position, Vector2.down, 0.1f, 1<<6);
        
        //Rotate enemy to face player
        int dirXToPlayer = (int)Mathf.Sign(target.transform.position.x - enemy.transform.position.x);
        if (isGrounded && dirXToPlayer != enemy.direction)
        {
            enemy.direction = dirXToPlayer;
            enemy.transform.Rotate(Vector2.up, 180f);
        }

        
        bool playerOnTop = target.position.y - enemy.transform.position.y > 2;
        bool shouldJump = playerOnTop && isGrounded && canJump;
        
        if (shouldJump)
        {
            Debug.Log("Starting jumpinggs");
            canJump = false;
            enemy.StartCoroutine(AllowJump());
            enemy.StartCoroutine(JumpTowardsClosestTile());
        }
        
        //Move forward
        enemy.transform.Translate(-Vector2.right * (moveSpeed * Time.deltaTime));
    }
    
    
    private IEnumerator AllowJump()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("jump tru");
        canJump = true;
    }

    /// <summary>
    /// Attempts to jump to the closest Tile
    /// </summary>
    private IEnumerator JumpTowardsClosestTile()
    {
        jumping = true;
        
        int dirXToClosestTile = (int)Mathf.Sign(GetClosestTile().position.x - enemy.transform.position.x);
        if(dirXToClosestTile != enemy.direction)
        {
            enemy.direction = dirXToClosestTile;
            enemy.transform.Rotate(Vector2.up, 180f);
        }
        
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => isGrounded);
        jumping = false;
    }
    
    /// <summary>
    /// Gets the closest tile which is above the enemy
    /// </summary>
    /// <returns></returns>
    private Transform GetClosestTile()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject tile in TileGen.instance.allCurrentTiles)
        {
            // Calculate the distance between the player and the current object
            float distance = Vector2.Distance(enemy.transform.position, tile.transform.position);
            bool tileOnTop = tile.transform.position.y > enemy.transform.position.y;
            
            // Check if the current object is closer than the previous closest object
            if (tileOnTop && distance < closestDistance) //Also check if the player is on top of the enemy then choose closes tile on top and vice-versa
            {
                closestDistance = distance;
                closestObject = tile;
            }
        }
        return closestObject.transform;
    }
}
