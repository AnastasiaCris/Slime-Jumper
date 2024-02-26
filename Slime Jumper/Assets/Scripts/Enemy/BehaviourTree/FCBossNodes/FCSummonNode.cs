using System.Collections.Generic;
using UnityEngine;

public class FCSummonNode : Node
{
    private FCBossBehaviour boss;
    private bool allEnemyTypes;
    private List<EnemyType> enemyTypes;
    private List<BaseEnemyBehaviour> allEnemies;
    private bool endOfSummon;
    private int nrOfSummons;

    private bool activated;
    private bool startSummon;
    public FCSummonNode(FCBossBehaviour boss, bool allEnemyTypes, List<EnemyType> enemyTypes, List<BaseEnemyBehaviour> allEnemies)
    {
        this.boss = boss;
        this.allEnemyTypes = allEnemyTypes;
        this.enemyTypes = enemyTypes;
        this.allEnemies = allEnemies;
    }
    public override NodeState Evaluate()
    {
        if (!startSummon)
        {
            startSummon = true;
            SummonEnemies();
        }
        
        return activated ? NodeState.FAILURE : NodeState.SUCCESS;
    }

    private void SummonEnemies()
    {
        if (allEnemyTypes)
        {
            for (int i = 0; i < allEnemies.Count; i++)
            {
                //activate all enemies
                allEnemies[i].ResetEnemy();//if there are dead skeletons
                allEnemies[i].StartEnemy();
            }
        }
        else
        {
            int random = Random.Range(0, enemyTypes.Count);
            EnemyType enemyType = enemyTypes[random];
            
            //activate random enemy type
            for (int i = 0; i < allEnemies.Count; i++)
            {
                if(allEnemies[i].EnemyScriptableObject.enemytype == enemyType){
                        allEnemies[i].StartEnemy();
                }
            }
        }
        
        if (allEnemyTypes)//stage 3
        {
            boss.stage3ActivationChecks++;
            if(boss.stage3ActivationChecks == 2)
                boss.SetStageActivation(3, true);
        }
        else//stage 2
        {
            boss.stage2ActivationChecks++;
            if(boss.stage2ActivationChecks == 2)
                boss.SetStageActivation(2, true);
        }
        
        activated = true;
    }
}
