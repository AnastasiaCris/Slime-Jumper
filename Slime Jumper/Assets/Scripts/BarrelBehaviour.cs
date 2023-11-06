using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelBehaviour : MonoBehaviour
{
    Animator barrelAnim;

    CapsuleCollider2D barrelCollider;
    private void Start()
    {
        barrelAnim = GetComponent<Animator>();
        barrelCollider = GetComponent<CapsuleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon")
        {
            barrelAnim.SetBool("IsDestroyed", true);
        }
    }
}
