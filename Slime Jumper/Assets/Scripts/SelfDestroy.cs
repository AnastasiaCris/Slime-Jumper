using System.Collections;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float secToDestroy;
    [SerializeField] private SpriteRenderer sprite;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            StartCoroutine(DestroyTile());
        }
    }

    IEnumerator DestroyTile()
    {
        WaitForSeconds waitSec = new WaitForSeconds(secToDestroy/6);
        
        sprite.color = Color.red;
        yield return waitSec;
        sprite.color = Color.white;
        yield return waitSec;
        sprite.color = Color.red;
        yield return waitSec;
        sprite.color = Color.white;
        yield return waitSec;
        sprite.color = Color.red;
        yield return waitSec;

        Destroy(gameObject);
    }
}
