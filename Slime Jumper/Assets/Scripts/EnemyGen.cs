using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGen : MonoBehaviour
{
    private TileGen tileGen;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float percentSpawnChance;
    void Start()
    {
        tileGen = GetComponent<TileGen>();
    }

    //whenever a new tile is spawned it has a certain percent chance to spawn an enemy
    public void SpawnEnemy(Vector2 tilePos)
    {
        if (Random.Range(0f, 1f) < percentSpawnChance)
        {
            
            Instantiate(enemy, tilePos, Quaternion.identity);
        }
    }
}
