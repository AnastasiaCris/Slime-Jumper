using System.Collections.Generic;
using UnityEngine;

public class FCSummonNode : Node
{
    private FCBossBehaviour boss;
    private bool allEnemyTypes;
    private List<EnemyType> enemyTypes;
    private List<SlimeBehaviour> allEnemies;
    private bool endOfSummon;
    private int nrOfSummons;
    public FCSummonNode(FCBossBehaviour boss, bool allEnemyTypes, List<EnemyType> enemyTypes, List<SlimeBehaviour> allEnemies)
    {
        this.boss = boss;
        this.allEnemyTypes = allEnemyTypes;
        this.enemyTypes = enemyTypes;
        this.allEnemies = allEnemies;
    }
    public override NodeState Evaluate()
    {
        throw new System.NotImplementedException();
    }

    private void SummonEnemies()
    {
        if (allEnemyTypes)
        {
            for (int i = 0; i < allEnemies.Count; i++)
            {
                //activate all enemies
                allEnemies[i].gameObject.SetActive(true);
            }
        }
        else
        {
            int random = Random.Range(0, enemyTypes.Count);
            EnemyType enemyType = enemyTypes[random];
            
            //activate random enemy type
            for (int i = 0; i < allEnemies.Count; i++)
            {
                //if(allEnemies[i].enemyType == enemyType){
                        //allEnemies[i].SetActive(true);
                //}
            }
        }
    }
}
