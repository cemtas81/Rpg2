using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class controllsprompt : MonoBehaviour
{
    float timeToMove;

    // Start is called before the first frame update
    void Start()
    {
        timeToMove = Time.time + 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            nextScene();
        }

        if (timeToMove <= Time.time)
        {
            nextScene();
        }
    }

    public void nextScene()
    {
        SceneManager.LoadScene(2);
    }
}
