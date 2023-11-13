
using UnityEngine;

public class TileDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            TileGen.instance.allCurrentTiles.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
