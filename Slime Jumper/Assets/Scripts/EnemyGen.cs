using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyGen : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform enemyParent;
    
    //Object pooling
    private List<GameObject> pooledEnemies = new List<GameObject>();
    private int amountToPool = 10;

    private void Awake()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(enemy, Vector3.zero, Quaternion.identity, enemyParent);
            obj.SetActive(false);
            pooledEnemies.Add(obj);
        }
    }

    //a certain percent chance to spawn an enemy
    public void SpawnEnemy(Vector2 tilePos)
    {
        if (Random.Range(0, 100) < enemy.GetComponent<EnemyBehaviour>().MyEnemy.spawnChance)
        {
            GameObject enemyObj = ReturnPooledObject(pooledEnemies);
            enemyObj.transform.position = tilePos;
            EnemyBehaviour enemyScript = enemyObj.GetComponent<EnemyBehaviour>();
            enemyObj.SetActive(true);
            if(enemyScript.patrollingSequence!= null && enemyScript.patrollingSequence.NodeState == NodeState.FAILURE) enemyScript.chasing = false;
            if (enemyScript.JUMPING) enemyScript.JUMPING = false;
            enemyScript.currentHP = enemyScript.maxHP;
        }
    }
    
    /// <summary>
    /// Returns an inactive object
    /// </summary>
    private GameObject ReturnPooledObject(List<GameObject> pooledList)
    {
        for (int i = 0; i < pooledList.Count; i++)
        {
            if (!pooledList[i].activeSelf)
            {
                return pooledList[i];
            }
        }
        return null;
    }
}
