using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;


public class MySolidSpawner : MonoBehaviour
{
    // The prefabs to spawn
    public GameObject[] prefabs;
    public float desiredCircleRadius;
    // The minimum and maximum number of prefabs to spawn
    public int minSpawnCount = 5;
    public int maxSpawnCount = 10;
    public int maxActivePrefabs = 65;
    // The movable object to generate spawn points around
    public GameObject movableObject;
    [Range(0.0f, 1f)]
    public float nadide;
    // The list of spawned prefabs
    public List<GameObject> spawnedPrefabs;
    public float spawnInterval;
    // The object pool
    public ObjectPooling2 objectPool;
    
    void Start()
    {
        // Initialize the list of spawned prefabs
        spawnedPrefabs = new List<GameObject>();
        Spawn2();
        // Start the spawn coroutine
        StartCoroutine(SpawnCoroutine());
  
    }

    IEnumerator SpawnCoroutine()
    {
        
        {
            // Loop indefinitely
            while (true)
            {
                // Wait for the specified interval
                yield return new WaitForSeconds(spawnInterval);

                // Spawn the prefabs
                int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
                for (int i = 0; i < spawnCount; i++)
                {
                    Spawn2();
                }
            }
        }
    }
    public void Spawn2()
    {
        // Choose the first prefab more often and the second prefab less often
        GameObject prefab;
        if (Random.value <= nadide)
        {
            prefab = prefabs[0];  // the first prefab has a higher chance of spawning 
        }
        else
        {
            prefab = prefabs[1];  // the second prefab has a lower chance of spawning
        }
        if (spawnedPrefabs.Count >= maxActivePrefabs )
        {
            return;
        }
        // Get a prefab from the object pool for the chosen prefab
        GameObject prefabToSpawn = objectPool.GetPooledObject(prefab);
        if (prefabToSpawn == null)
        {
            return;
        }
     
        // Generate a random position within the desired circle radius around the movable object
        float angle = Random.Range(0f, 360f);
        float x = movableObject.transform.position.x + desiredCircleRadius * Mathf.Cos(angle);
        float y = movableObject.transform.position.y;
        float z = movableObject.transform.position.z + desiredCircleRadius * Mathf.Sin(angle);
        Vector3 position = new(x, y, z);

        // Set the position and rotation of the prefab
        prefabToSpawn.transform.position = position;
        prefabToSpawn.transform.rotation = Quaternion.identity;
        //prefabToSpawn.transform.parent = transform;
        // Enable the prefab
        prefabToSpawn.SetActive(true);     
        // Add the prefab to the list of spawned prefabs
        spawnedPrefabs.Add(prefabToSpawn);
        
    }
}


