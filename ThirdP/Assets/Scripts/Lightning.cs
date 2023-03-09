using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public static Lightning instance;

    public GameObject particles;

    private float despawnTime;

    public AudioSource lightningSFX;
    public AudioClip[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;

        despawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(despawnTime <= Time.time)
        {
            particles.SetActive(false);
        }
    }

    public void spawn(Vector3 location)
    {
        location.y = location.y + 5;
        this.transform.position = location;
        particles.SetActive(true);
        playAudio();
        despawnTime = Time.time + 1f;
    }

    private void playAudio()
    {
        lightningSFX.clip = audioSources[Random.Range(0, audioSources.Length)];
        lightningSFX.Play();
    }
}
