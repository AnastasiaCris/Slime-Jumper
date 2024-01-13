using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyGen : Generate<SlimeBehaviour>
{
    //A certain percent chance to spawn an enemy
    public void AttemptEnemySpawn(Vector2 tilePos)
    {
        for (int i = 0; i < objPrefabs.Length; i++)
        {
            if (Random.Range(0, 100) < objPrefabs[i].EnemyScriptableObject.spawnChance)
            {
                SlimeBehaviour slimeScript = ReturnPooledObject(i);
                slimeScript.transform.position = tilePos;
                slimeScript.ResetEnemy();
            }
        }
    }
}
