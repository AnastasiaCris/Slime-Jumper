using UnityEngine;

public class GameProperties : MonoBehaviour
{
    public static Transform playerLoc { get; private set; }
    public static Player playerScript { get; private set; }
    public static Camera cam { get; private set; }
    public static float screenWidth { get; private set; }

    private void Awake()
    {
        playerLoc = GameObject.FindWithTag("Player").transform;
        playerScript = playerLoc.GetComponent<Player>();
        cam = Camera.main;
        screenWidth = cam.orthographicSize * 2 * Screen.width / Screen.height;
    }
}
