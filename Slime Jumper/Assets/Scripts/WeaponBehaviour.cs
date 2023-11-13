using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{

    [SerializeField] Player player;

    [SerializeField] BarrelBehaviour barrelBehaviour;

    [Header("Weapons")]

    [SerializeField] BoxCollider2D weaponCollider;
    [SerializeField] Animator weaponAnimator;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        weaponCollider = GetComponent<BoxCollider2D>();
        weaponAnimator = GetComponent<Animator>();
        //weaponCollider.enabled = !weaponCollider.enabled;
    }

    public void AttackColliderOn()
    {
        //Set IsAttacking to true
        //make weapon's attack counter = player's animator's attack counter
        //Set colider to enabled 
        weaponCollider.enabled = true;
        weaponAnimator.SetBool("IsAttacking", true);
        weaponAnimator.SetInteger("Attack Counter", player.attackCounter);
        if (weaponAnimator.GetInteger("Attack Counter") > 2)
        {
            weaponAnimator.SetInteger("Attack Counter", 0);
        }

        Debug.Log("WeaponOn");
    }

    public void AttackColliderOff()
    {
        //Set IsAttacking to false
        //diable collider

        weaponCollider.enabled = false;
        weaponAnimator.SetBool("IsAttacking", false);
        Debug.Log("WeaponOff");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Objects")
        {
            barrelBehaviour.ObjectCollapse();
        }
        Debug.Log("Triggerworked");
    }
}
