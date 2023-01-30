using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMultiSpawner : MonoBehaviour

{
    // Prefab for the enemy objects
    public GameObject[] enemyPrefabs;

    // Array to store enemy objects in the object pool
    GameObject[][] enemyPools;

    // Number of enemy objects to store in the object pool
    public int poolSize;
    public Transform Player;
    [SerializeField]
    private float smallinterval;
    [SerializeField]
    public float largeinterval;
    private Transform place;
    void Start()
    {
        // Initialize the object pools with the specified number of enemy objects
        enemyPools = new GameObject[enemyPrefabs.Length][];
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyPools[i] = new GameObject[poolSize];
            for (int j = 0; j < poolSize; j++)
            {
                enemyPools[i][j] = Instantiate(enemyPrefabs[i]);
                enemyPools[i][j].SetActive(false);
            }
        }
        StartCoroutine(Spawnen(smallinterval, 0));
        StartCoroutine(Spawnen(largeinterval, 1));
    }
    private IEnumerator Spawnen(float interval,int enemyType)
    {
        yield return new WaitForSeconds(interval);
        place = Player;
        SpawnEnemy(enemyType);
        StartCoroutine(Spawnen(interval,enemyType));
    }
    // Function to spawn a new enemy of a specified type
    public void SpawnEnemy(int enemyType)
    {
        // Find the first inactive enemy object in the object pool
        GameObject enemy = null;
        for (int i = 0; i < poolSize; i++)
        {
            if (!enemyPools[enemyType][i].activeInHierarchy)
            {
                enemy = enemyPools[enemyType][i];
                break;
            }
        }

        // If an inactive enemy object was found, activate it and set its position
        if (enemy != null)
        {
            enemy.SetActive(true);
            enemy.GetComponent<CapsuleCollider>().enabled = true;
            enemy.transform.position =new Vector3(Random.Range(place.position.x-10,place.position.x+10),place.position.y,place.position.z+25);
        }
    }
}


