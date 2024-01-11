using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCActivateTiles : Node
{
    private FCBossBehaviour boss;
    private List<FCSpecialTile> specialTiles = new List<FCSpecialTile>();
    private bool activateAll;
    
    private bool activated;
    private bool startActivation;

    public FCActivateTiles(FCBossBehaviour boss, bool activateAll, List<FCSpecialTile> specialTiles)
    {
        this.boss = boss;
        this.activateAll = activateAll;
        this.specialTiles = specialTiles;
    }

    public override NodeState Evaluate()
    {
        if (!startActivation)
        {
            startActivation = true;
            ActivateTiles();
        }

        return activated ? NodeState.FAILURE : NodeState.SUCCESS;
    }

    private void ActivateTiles()
    {
        if (activateAll)
        {
            foreach (var tile in specialTiles)
            {
                tile.TriggerTile();
            }
        }
        else
        {
            int random = Random.Range(1, specialTiles.Count - 1); //random amount of tiles (but never all of them or none)
            for (int i = 0; i < random; i++)
            {
                specialTiles[i].TriggerTile();
            }
        }

        activated = true;
    }
}
