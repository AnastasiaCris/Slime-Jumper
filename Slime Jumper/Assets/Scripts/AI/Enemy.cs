using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy") ]
public class Enemy : ScriptableObject
{
    public int maxHP;
    public float damage;
    public float speed;
    public float jumpHeight;
    public float secUntilNextAttack;
}
