using UnityEngine;

public class TileGen : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Transform cameraTopPoint;
    [SerializeField] private float maxPlatformDistance = 2f;
    [SerializeField] private float minPlatformDistance = 0.2f;

    private float screenWidth;

    void Start()
    {
        screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        
    }

    void Update()
    {
        if (cameraTopPoint.position.y > generationPoint.position.y)
        {
            GeneratePlatform();
        }
    }

    void GeneratePlatform()
    {
        //Random Dist between platforms
        float platformDistance = Random.Range(minPlatformDistance, maxPlatformDistance);
        
        // Generate a platform at a random X position within the screen width.
        float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
        
        Vector3 platformPosition = new Vector3(randomX, generationPoint.position.y, 0);

        Instantiate(platformPrefab, platformPosition, Quaternion.identity);

        generationPoint.position = new Vector3(randomX, generationPoint.position.y + platformDistance, 0);
    }
}
