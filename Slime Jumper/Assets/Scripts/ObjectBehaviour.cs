using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    public int spawnChance = 20;
    [SerializeField]private Animator barrelAnim;

    private void Awake()
    {
        barrelAnim = GetComponent<Animator>();
    }
    public void ObjectCollapse()
    {
        barrelAnim.SetBool("isDestroyed", true);
    }

    public void ResetObject()
    {
        barrelAnim.SetBool("isDestroyed", false);
    }
}
