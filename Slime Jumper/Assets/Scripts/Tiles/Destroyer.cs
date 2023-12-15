
using UnityEngine;

public class Destroyer : Tile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            TileGen.instance.allCurrentTiles.Remove(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            //kill player
            //reset game - happens in death animation
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if(playerScript != null)
                playerScript.ResetScene();
        }
        else
        {
            collision.gameObject.SetActive(false);
        }
    }
}
