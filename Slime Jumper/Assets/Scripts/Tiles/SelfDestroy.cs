using System.Collections;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float secToDestroy;
    [SerializeField] private float shakeIntensity;
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            StartCoroutine(DestroyTile());
        }
    }

    IEnumerator DestroyTile()
    {
        float currentSec = 0;
        Vector2 originalPosition = transform.position;

        //shake
        while (currentSec <= secToDestroy)
        {
            Vector2 offset = new Vector2(Random.Range(-1f, 1f) * shakeIntensity, 0);
            transform.position = originalPosition + offset;

            currentSec += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        
        //move down
        while (gameObject.activeSelf)
        {
            transform.Translate(Vector2.down * (10 * Time.deltaTime)); 
            yield return null;
        }

        TileGen.instance.allCurrentTiles.Remove(gameObject);
    }
}
