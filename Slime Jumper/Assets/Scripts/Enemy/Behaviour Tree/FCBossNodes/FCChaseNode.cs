using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCChaseNode : Node
{
    //if not on the same tile as the player it is a success

    private GameObject playerTile;
    private GameObject bossTile;
    private GameObject bossObj;

    public FCChaseNode(GameObject playerTile,GameObject bossTile)
    {
        this.playerTile = playerTile;
        this.bossTile = bossTile;
    }
    public override NodeState Evaluate()
    {
        if (playerTile != bossTile)
        {
            TeleportToPlayer();
            return NodeState.SUCCESS;
        }
        
        return NodeState.FAILURE;
    }

    /// <summary>
    /// Teleports on the same tile as the player in a random x position (but close to player)
    /// </summary>
    private void TeleportToPlayer()
    {
        Vector2 newPos = playerTile.transform.position;
        float newX = Random.Range(newPos.x - 2, newPos.x + 2);
        newPos = new Vector2(newX, newPos.y);
        bossObj.transform.position = newPos;
    }
}
