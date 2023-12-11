
using UnityEngine;

public class MovingTile : MonoBehaviour
{
    private float speed;
    private float leftBoundary;
    private float rightBoundary;
    private float threshold;
    private int dir = 1;
    void Start()
    {
        leftBoundary = Random.Range(-5, 5);
        threshold = Random.Range(5, 10);
        rightBoundary = leftBoundary + threshold;
        
        speed = Random.Range(1f, 3);
    }

    void Update()
    {
        MoveTile();
    }

    private void MoveTile()
    {
        // Move the tile
        transform.Translate(Vector3.right * speed * dir * Time.deltaTime);

        // Check if the tile is beyond the right boundary
        if (transform.position.x >= rightBoundary)
        {
            dir = -1; // Change direction to move left
        }

        // Check if the tile is beyond the left boundary
        if (transform.position.x <= leftBoundary)
        {
            dir = 1; // Change direction to move right
        }
    }
}
