using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void BreakPower()
    {
        Debug.Log(gameObject.name);
        Debug.Log("BreakPower");
    }

    public virtual void StartTelekinesisPower(Camera camera)
    {
        Debug.Log(gameObject.name);
        Debug.Log("StartTelekinesisPower");
    }

    public virtual void StopTelekinesisPower()
    {
        Debug.Log(gameObject.name);
        Debug.Log("StopTelekinesisPower");
    }
}
