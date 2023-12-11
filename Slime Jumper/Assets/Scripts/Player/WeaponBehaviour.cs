using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] Player player;
    [Header("Weapons")]
    public List<EnemyBehaviour> enemiesInAttackRange = new List<EnemyBehaviour>();
    public List<ObjectBehaviour> objectsInAttackRange = new List<ObjectBehaviour>();

    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Objects"))
        {
            ObjectBehaviour objScript = other.GetComponent<ObjectBehaviour>();
            if(objScript != null)
                objectsInAttackRange.Add(objScript);
            
        }else if (other.CompareTag("Enemy"))
        {
            EnemyBehaviour enemyScript = other.GetComponent<EnemyBehaviour>();
            if(enemyScript != null)
                enemiesInAttackRange.Add(enemyScript);
            
            RemoveDuplicates(enemiesInAttackRange); //in case the same enemy has 2 colliders
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBehaviour enemyScript = other.GetComponent<EnemyBehaviour>();
            if(enemyScript != null)
                enemiesInAttackRange.Remove(enemyScript);
        }else if (other.CompareTag("Objects"))
        {
            ObjectBehaviour objScript = other.GetComponent<ObjectBehaviour>();
            if(objScript != null)
                objectsInAttackRange.Remove(objScript);
        }
    }
    
    /// <summary>
    /// Removes any duplicates from a list
    /// </summary>
    private void RemoveDuplicates<T>(List<T> list)
    {
        // Use LINQ to remove duplicates
        List<T> uniqueList = list.Distinct().ToList();

        // Clear the original list and add unique elements back
        list.Clear();
        list.AddRange(uniqueList);
    }
    
}
