using UnityEngine;

public class ObjectGen : Generate<ObjectBehaviour>
{
    //A certain percent chance to spawn an object
    public void SpawnObject(Vector2 tilePos)
    {
        for (int i = 0; i < objPrefabs.Length; i++)
        {
            if (Random.Range(0, 100) < 20)
            {
                ObjectBehaviour objScript = ReturnPooledObject(i);
                tilePos = new Vector2(tilePos.x, tilePos.y + 0.7f);
                objScript.transform.position = tilePos;
                objScript.gameObject.SetActive(true);
                objScript.ResetObject();
            }

        }
    }
}
