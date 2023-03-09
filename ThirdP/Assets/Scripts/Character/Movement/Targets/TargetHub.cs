using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetHub : MonoBehaviour
{
    public static TargetHub instance;

    public GameObject[] targets;

    public NavMeshSurface surface;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        //This should be destroyed on load though as the one in the next level will take over.
    }

    // Start is called before the first frame update
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("PointOfInterest");
        Debug.Log(targets.Length + " targets found.");

        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Target getRandom()
    {
        return targets[Random.Range(0, targets.Length)].GetComponent<Target>();
    }
}
