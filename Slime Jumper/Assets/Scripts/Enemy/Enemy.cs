using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy") ]
public class Enemy : ScriptableObject
{
    public int maxHP;
    public int damage;
    public float speed;
    public float jumpHeight;
    public float secUntilNextAttack;
    public int spawnChance;
}
