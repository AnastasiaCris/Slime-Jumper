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
    public void ObjectCollapse()
    {

        barrelAnim.SetBool("IsDestroyed", true);

    }
}
