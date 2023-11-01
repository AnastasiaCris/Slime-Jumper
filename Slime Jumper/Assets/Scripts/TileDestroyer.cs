
using UnityEngine;

public class TileDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            Destroy(collision.gameObject);
        }
    }
}
