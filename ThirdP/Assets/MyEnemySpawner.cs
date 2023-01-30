using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyEnemySpawner : MonoBehaviour
{
    // Prefab for the enemy objects
    public GameObject enemyPrefab;

    // Array to store enemy objects in the object pool
    GameObject[] enemyPool;

    // Number of enemy objects to store in the object pool
    public int poolSize = 10;

    void Start()
    {
        // Initialize the object pool with the specified number of enemy objects
        enemyPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            enemyPool[i] = Instantiate(enemyPrefab);
            enemyPool[i].SetActive(false);
        }
    }

    // Function to spawn a new enemy
    public void SpawnEnemy()
    {
        // Find the first inactive enemy object in the object pool
        GameObject enemy = null;
        for (int i = 0; i < poolSize; i++)
        {
            if (!enemyPool[i].activeInHierarchy)
            {
                enemy = enemyPool[i];
                break;
            }
        }

        // If an inactive enemy object was found, activate it and set its position
        if (enemy != null)
        {
            enemy.SetActive(true);
            enemy.transform.position = transform.position;
        }
    }
}
