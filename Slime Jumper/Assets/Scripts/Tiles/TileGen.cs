using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGen : Generate<Tile>
{
    
    [Header("Generation Properties")]
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Transform cameraTopPoint;
    [SerializeField] private float maxPlatformDistance = 2f;
    [SerializeField] private float minPlatformDistance = 0.2f;
    public List<GameObject> allCurrentTiles = new List<GameObject>();
    private float screenWidth;

    [Header("Other")]
    public static TileGen instance;
    private EnemyGen enemyGen;
    private ObjectGen objectGen;

    public override void Awake()
    {
        base.Awake();
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        enemyGen = GetComponent<EnemyGen>();
        objectGen = GetComponent<ObjectGen>();
    }

    void Update()
    {
        if (cameraTopPoint.position.y > generationPoint.position.y)
        {
            GeneratePlatform();
        }
    }

    private void GeneratePlatform()
    {
        //Random Dist between platforms
        float platformDistance = Random.Range(minPlatformDistance, maxPlatformDistance);
        
        // Random X position within the screen width.
        float randomX = Random.Range(-screenWidth / 2 + 3, screenWidth / 2 - 3);
        Vector3 platformPosition = new Vector3(randomX, generationPoint.position.y, 0);
        
        //Generate Tile
        GameObject newTile = GenerateRandomTile();
        newTile.transform.position = platformPosition;
        allCurrentTiles.Add(newTile);
        
        //Try spawning enemy
        enemyGen.SpawnEnemy(platformPosition);
        
        //Try spawning object
        objectGen.SpawnObject(platformPosition);

        //Set the y pos of the next tile
        generationPoint.position = new Vector3(randomX, generationPoint.position.y + platformDistance, 0);
        
    }

    //Tile Type: 0 - normal, 1 - self-destroy, 2 - moving
    
    private GameObject GenerateRandomTile()
    {
        if (Random.Range(0, 100) < objPrefabs[1].spawnChance)
        {
            GameObject newTile = ReturnPooledObject(1).gameObject;
            newTile.GetComponent<SelfDestroy>().StopAllCoroutines();
            newTile.SetActive(true);
            return newTile;
        }else if (Random.Range(0, 100) < objPrefabs[2].spawnChance)
        {
            GameObject newTile = ReturnPooledObject(2).gameObject;
            newTile.SetActive(true);
            return newTile;
        }
        
        GameObject normalTile = ReturnPooledObject(0).gameObject;
        normalTile.SetActive(true);
        return normalTile;

    }
}
