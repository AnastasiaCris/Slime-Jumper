using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGen : MonoBehaviour
{
    [Header("Tile Type")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject destroyingTilePrefab;
    [SerializeField] private GameObject movingTilePrefab;
    
    [Header("Generation Properties")]
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Transform cameraTopPoint;
    [SerializeField] private Transform tileParent;
    [SerializeField] private float maxPlatformDistance = 2f;
    [SerializeField] private float minPlatformDistance = 0.2f;
    public List<GameObject> allCurrentTiles = new List<GameObject>();
    private float screenWidth;
    
    [Header("Spawn Chances")]
    [SerializeField] private float selfDestroyTileSpawnChance = 25f;
    [SerializeField] private float movingTileSpawnChance = 10f;

    [Header("Object Pooling")]
    private List<GameObject> pooledTiles = new List<GameObject>();
    private List<GameObject> pooledDestroyingTiles = new List<GameObject>();
    private List<GameObject> pooledMovingTiles = new List<GameObject>();
    private int amountToPool = 30;
    
    [Header("Other")]
    public static TileGen instance;
    private EnemyGen enemyGen;
    private ObjectGen objectGen;

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
        objectGen = GetComponent<ObjectGen>();

        for (int i = 0; i < amountToPool; i++)
        {
            //Simple Tiles
            GameObject tile = Instantiate(platformPrefab, Vector3.zero, Quaternion.identity, tileParent);
            tile.SetActive(false);
            pooledTiles.Add(tile);
            
            //Destroying Tiles
            GameObject destroyTile = Instantiate(destroyingTilePrefab, Vector3.zero, Quaternion.identity, tileParent);
            destroyTile.SetActive(false);
            pooledDestroyingTiles.Add(destroyTile);
            
            //Moving Tiles
            GameObject movingTiles = Instantiate(movingTilePrefab, Vector3.zero, Quaternion.identity, tileParent);
            movingTiles.SetActive(false);
            pooledMovingTiles.Add(movingTiles);
        }
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

    private GameObject GenerateRandomTile()
    {
        if (Random.Range(0, 100) < selfDestroyTileSpawnChance)
        {
            GameObject newTile = ReturnPooledObject(pooledDestroyingTiles);
            newTile.GetComponent<SelfDestroy>().StopAllCoroutines();
            newTile.SetActive(true);
            return newTile;
        }

        if (Random.Range(0, 100) < movingTileSpawnChance)
        {
            GameObject newTile = ReturnPooledObject(pooledMovingTiles);
            newTile.SetActive(true);
            return newTile;
        }

        GameObject normalTile = ReturnPooledObject(pooledTiles);
        normalTile.SetActive(true);
        return normalTile;
    }

    /// <summary>
    /// Returns an inactive object
    /// </summary>
    private GameObject ReturnPooledObject(List<GameObject> pooledList)
    {
        for (int i = 0; i < pooledList.Count; i++)
        {
            if (!pooledList[i].activeSelf)
            {
                return pooledList[i];
            }
        }
        return null;
    }
}
