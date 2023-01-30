using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling2 : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int initialPoolSize = 10;
        public List<GameObject> pooledObjects;
    }

    public List<Pool> pools; // List of pools, one for each prefab

    void Start()
    {
        // Initialize each pool
        foreach (Pool pool in pools)
        {
            pool.pooledObjects = new List<GameObject>();
            for (int i = 0; i < pool.initialPoolSize; i++)
            {
                GameObject obj = (GameObject)Instantiate(pool.prefab);
                obj.SetActive(false);
                pool.pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(GameObject prefab)
    {
        // Find the pool for the desired prefab
        foreach (Pool pool in pools)
        {
            if (pool.prefab == prefab)
            {
                // Check for an inactive object in the pool
                for (int i = 0; i < pool.pooledObjects.Count; i++)
                {
                    if (!pool.pooledObjects[i].activeInHierarchy)
                    {
                        return pool.pooledObjects[i];
                    }
                }

                // If no inactive objects, instantiate a new one
                GameObject obj = (GameObject)Instantiate(prefab);
                pool.pooledObjects.Add(obj);
                return obj;
            }
        }

        // If the prefab is not found in the pool, return null
        return null;
    }
    //public void ReturnToPool(GameObject obj)
    //{
    //    // Find the pool for the object
    //    foreach (Pool pool in pools)
    //    {
    //        if (pool.pooledObjects.Contains(obj))
    //        {
    //            obj.SetActive(false);               
    //            return;
    //        }
    //    }
    //}

}

