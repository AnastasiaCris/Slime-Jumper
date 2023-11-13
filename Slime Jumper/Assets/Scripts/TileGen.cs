using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGen : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Transform cameraTopPoint;
    [SerializeField] private float maxPlatformDistance = 2f;
    [SerializeField] private float minPlatformDistance = 0.2f;
    private float screenWidth;
    public List<GameObject> allCurrentTiles = new List<GameObject>();

    public static TileGen instance;

    private EnemyGen enemyGen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        enemyGen = GetComponent<EnemyGen>();
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

        allCurrentTiles.Add(Instantiate(platformPrefab, platformPosition, Quaternion.identity));
        
        //generate enemy
        enemyGen.SpawnEnemy(platformPosition);

        generationPoint.position = new Vector3(randomX, generationPoint.position.y + platformDistance, 0);
        
    }
}
