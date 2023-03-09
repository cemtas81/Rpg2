using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    public int sceneToLoad;

    DialogueManager DM;

    // Start is called before the first frame update
    void Start()
    {
        DM = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        if (DM != null)
        {
            DM.startDialogue(false);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (DM != null)
            {
                DM.startDialogue(true);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            
        }
    }
}
