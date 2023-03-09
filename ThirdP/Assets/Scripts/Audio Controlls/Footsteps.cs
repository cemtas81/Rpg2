using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private float speed;
    private Vector3 lastPosition;
    public AudioSource audio;

    public Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curMove = transform.position - lastPosition;
        speed = curMove.magnitude / Time.deltaTime;
        lastPosition = transform.position;

        ani.SetFloat("speed", speed);

        if (speed > 2f)
        {
            audio.mute = false;
        } else
        {
            audio.mute = true;
        }
    }

}
