using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavigationBaker : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
