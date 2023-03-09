﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupTypes type;
    public float destroyTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var MCS = other.GetComponent<MainCharacterScript>();
        if (MCS != null && other.CompareTag("Player"))
        {
            MCS.Pickup(type);
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(gameObject, destroyTime);
        }    
    }
}

public enum PickupTypes 
{
    key
}
