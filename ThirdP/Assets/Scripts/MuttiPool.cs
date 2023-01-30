using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MuttiPool : MonoBehaviour
{
    // A dictionary to hold the object pools
    public Dictionary<string, ArrayList> objectPools;
    public static MuttiPool SharedInstance;
    private void Awake()
    {
        SharedInstance = this;
    }
    // Function to create an object pool
    public void CreatePool(GameObject prefab, int poolSize)
    {
        // Create the object pool
        ArrayList objectPool = new ArrayList(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            // Instantiate the object and add it to the pool
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Add(obj);
        }

        // Add the object pool to the dictionary
        objectPools.Add(prefab.name, objectPool);
    }

    // Function to retrieve an object from the pool
    public GameObject GetPooledObject(string objectName)
    {
        // Get the object pool from the dictionary
        ArrayList objectPool = objectPools[objectName];

        // Iterate through the pool and find an inactive object
        for (int i = 0; i < objectPool.Count; i++)
        {
            GameObject obj = (GameObject)objectPool[i];
            if (!obj.activeInHierarchy)
            {
                // Return the inactive object
                return obj;
            }
        }

        // If there are no inactive objects, create a new one and add it to the pool
        GameObject prefab = (GameObject)Resources.Load(objectName);
        GameObject newObj = (GameObject)Instantiate(prefab);
        objectPool.Add(newObj);
        return newObj;
    }
}

