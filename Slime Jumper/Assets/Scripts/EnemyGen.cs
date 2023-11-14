using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGen : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform enemyParent;

    //a certain percent chance to spawn an enemy
    public void SpawnEnemy(Vector2 tilePos)
    {
        if (Random.Range(0, 100) < enemy.GetComponent<EnemyBehaviour>().MyEnemy.spawnChance)
        {
            Instantiate(enemy, tilePos, Quaternion.identity, enemyParent);
        }
    }
}
