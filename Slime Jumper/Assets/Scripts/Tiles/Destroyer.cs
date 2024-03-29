
using UnityEngine;

public class Destroyer : MonoBehaviour
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
            Player playerScript = GameProperties.playerScript;
            if(playerScript != null)
                playerScript.ResetScene();
        }
        else
        {
            collision.gameObject.SetActive(false);
        }
    }
}
