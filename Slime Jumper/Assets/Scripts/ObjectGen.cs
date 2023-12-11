using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGen : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform objectParent;
    
    //Object pooling
    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 10;

    private void Awake()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity, objectParent);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    //a certain percent chance to spawn an object
    public void SpawnObject(Vector2 tilePos)
    {
        if (Random.Range(0, 100) < 20)
        {
            GameObject obj = ReturnPooledObject(pooledObjects);
            ObjectBehaviour objScript = obj.GetComponent<ObjectBehaviour>();
            tilePos = new Vector2(tilePos.x, tilePos.y + 0.7f);
            obj.transform.position = tilePos;
            obj.SetActive(true);
            objScript.ResetObject();
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
