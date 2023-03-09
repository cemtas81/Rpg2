using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyCheck : MonoBehaviour
{
    private Animator ani;

    public GameObject blockerCube;

    private void Start()
    {
        ani = transform.gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        var MCS = other.GetComponent<MainCharacterScript>();
        if (MCS != null && other.CompareTag("Player"))
        {
            if (MCS.CheckForKey())
            {
                Debug.Log("OpenDoor");
                ani.SetTrigger("open"); //this animation is not working
                Destroy(blockerCube);
            }
        }
    }
}
