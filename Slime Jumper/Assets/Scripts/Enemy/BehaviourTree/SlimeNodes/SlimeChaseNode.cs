using System.Collections;
using UnityEngine;

public class SlimeChaseNode : Node
{
    private Transform target;
    private SlimeBehaviour _slime;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform groundPos;
    private float jumpForce;
    private float moveSpeed;
    private bool isGrounded;
    private bool canJump = true;

    public SlimeChaseNode(Transform target, SlimeBehaviour slime, EnemyScriptableObject enemyScriptableObjectStats,Transform groundPos)
    {
        this.target = target;
        this._slime = slime;
        this.groundPos = groundPos;
        jumpForce = enemyScriptableObjectStats.jumpHeight;
        moveSpeed = enemyScriptableObjectStats.speed;
        anim = slime.Anim;
        rb = slime.GetComponent<Rigidbody2D>();
    }
    
    public override NodeState Evaluate()
    {
        float distance = Vector2.Distance(target.position, _slime.transform.position);
        
        if (distance > 1.4f)//so it doesn't overlap with the player
        {
            FollowPlayer();

            if (_slime.State != State.Chasing)
            {
                _slime.ChangeState(State.Chasing);
                _slime.chasing = true;
            }
            anim.SetTrigger("chasing");

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
        isGrounded = Physics2D.OverlapCircle(groundPos.position, 0.5f, 1 << 6);

        //Rotate enemy to face player
        int dirXToPlayer = (int)Mathf.Sign(target.transform.position.x - _slime.transform.position.x);
        bool playerFullyOnTop = target.position.y - _slime.transform.position.y > 1;
        bool playerOnTop = target.position.y - _slime.transform.position.y > 0;
        bool playerRightUnder = !playerOnTop && (target.position.x - _slime.transform.position.x > 1 || target.position.x - _slime.transform.position.x < -1);
        bool tryReachingPlayer = playerOnTop || playerRightUnder;
        
        if (isGrounded && !_slime.jumping && tryReachingPlayer && dirXToPlayer != _slime.direction)
        {
            _slime.direction = dirXToPlayer;
            _slime.transform.Rotate(Vector2.up, 180f);
        }

        
        bool shouldJump = playerFullyOnTop && isGrounded && canJump;
        
        if (shouldJump)
        {
            canJump = false;
            _slime.StartCoroutine(JumpTowardsClosestTile());
        }
        
        //Move forward
        _slime.transform.Translate(-Vector2.right * (moveSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Attempts to jump to the closest Tile
    /// </summary>
    private IEnumerator JumpTowardsClosestTile()
    {
        _slime.jumping = true;//debug

        float distToClosestTile;
        int dirXToClosestTile = (int)Mathf.Sign(GetClosestTile(out distToClosestTile).position.x - _slime.transform.position.x);

        if (distToClosestTile < 13) // if close enough to the tile to jump
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //jump

            yield return new WaitUntil(() => !isGrounded);


            //rotate enemy to face closest tile
            while (!isGrounded)
            {
                if (dirXToClosestTile != _slime.direction)
                {
                    _slime.direction = dirXToClosestTile;
                    _slime.transform.Rotate(Vector2.up, 180f);
                }

                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitUntil(() => isGrounded);
        _slime.jumping = false; //debug

        yield return new WaitForSeconds(0.2f);
        canJump = true;
    }

    /// <summary>
    /// Gets the closest tile which is above the enemy
    /// </summary>
    /// <returns></returns>
    private Transform GetClosestTile(out float dist)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject tile in TileGen.instance.allCurrentTiles)
        {
            // Calculate the distance between the player and the current object
            float distance = Vector2.Distance(_slime.transform.position, tile.transform.position);
            bool tileOnTop = tile.transform.position.y > _slime.transform.position.y;
            bool tileReachable = tile.transform.position.y - _slime.transform.position.y <= 4;
            
            // Check if the current object is closer than the previous closest object
            if (tileOnTop && distance < closestDistance && tileReachable) //Also check if the player is on top of the enemy then choose closes tile on top and vice-versa
            {
                closestDistance = distance;
                closestObject = tile;
            }
        }

        dist = closestDistance;
        if (closestObject == null) return null;
        return closestObject.transform;
    }
}
