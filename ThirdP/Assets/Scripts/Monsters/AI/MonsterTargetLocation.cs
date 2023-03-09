using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTargetLocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(0.25f, 0.25f, 0.25f));
    }
}
