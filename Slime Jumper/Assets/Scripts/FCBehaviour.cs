using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCBehaviour : MonoBehaviour
{

    [Header("FC Elements")]

    Rigidbody2D fCRigidbody2D;
    Animator fCAnimator;

    CapsuleCollider2D fCBodyCollider;
    // Start is called before the first frame update
    void Start()
    {
        fCRigidbody2D = GetComponent<Rigidbody2D>();
        fCAnimator = GetComponent<Animator>();
        fCBodyCollider = GetComponent<CapsuleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
