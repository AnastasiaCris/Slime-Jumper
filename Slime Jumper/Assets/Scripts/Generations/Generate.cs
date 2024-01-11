using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Generate<T> : MonoBehaviour where T : MonoBehaviour
{
    //Object pooling
    [SerializeField] protected T[] objPrefabs;
    [SerializeField] private Transform objParent;
    [SerializeField] private int poolAmount = 10;

    private List<Queue<T>> objPoolList = new List<Queue<T>>();
    private List<Queue<T>> activeObjList = new List<Queue<T>>();
    
    //------------------------------Object Pooling---------------------------------

    public virtual void Awake()
    {
        for (int i = 0; i < objPrefabs.Length; i++)
        {
            Queue<T> objPool = new Queue<T>();
            Queue<T> activObj = new Queue<T>();
            objPoolList.Add(objPool);
            activeObjList.Add(activObj);
            PoolObjects(i);
        }
    }

    /// <summary>
    /// Returns the first inactive object type and turns it to active
    /// </summary>
    protected T ReturnPooledObject(int objType)
    {
        if (objPoolList[objType].Count == 0)
        {
            PoolObjects(objType);
        }

        T newObj = objPoolList[objType].Dequeue();
        activeObjList[objType].Enqueue(newObj);

        newObj.gameObject.SetActive(true);

        return newObj;
    }

    private void PoolObjects(int objType)
    {
        for (int i = 0; i < poolAmount; i++)
        {
            T objClone = Instantiate(objPrefabs[objType], Vector3.zero, Quaternion.identity, objParent);
            
            objClone.gameObject.SetActive(false);
            objPoolList[objType].Enqueue(objClone);
        }
    }
}

