using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy") ]
public class EnemyScriptableObject : ScriptableObject
{
    public EnemyType enemytype;
    public int maxHP;
    public int damage;
    public float speed;
    public float jumpHeight;
    public float secUntilNextAttack;
    public int spawnChance;
}
