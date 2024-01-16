using UnityEngine;

public class FCSpecialTile : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private int triggerAnimHash;
    private bool playerInTrigger;
    public bool tileTriggered;
    void Start()
    {
        triggerAnimHash = Animator.StringToHash("trigger");
    }

    public void TriggerTile()
    {
        tileTriggered = true;
        anim.SetTrigger(triggerAnimHash);
    }

    //called in the open animation
    private void TileOpened()
    {
        if (!playerInTrigger) return;
        GameProperties.playerScript.SlowDown(0.5f, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && tileTriggered)
        {
            playerInTrigger = true;
            TileOpened();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && tileTriggered)
        {
            playerInTrigger = false;
            GameProperties.playerScript.UnSlow();
        }
    }
}
