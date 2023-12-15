using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyGen : Generate<EnemyBehaviour>
{
    //A certain percent chance to spawn an enemy
    public void SpawnEnemy(Vector2 tilePos)
    {
        for (int i = 0; i < objPrefabs.Length; i++)
        {
            if (Random.Range(0, 100) < objPrefabs[i].EnemyScriptableObject.spawnChance)
            {
                EnemyBehaviour enemyScript = ReturnPooledObject(i).GetComponent<EnemyBehaviour>();
                enemyScript.transform.position = tilePos;
                enemyScript.ResetEnemy();
            }
        }
    }
}
