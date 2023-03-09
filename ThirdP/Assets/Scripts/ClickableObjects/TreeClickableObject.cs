using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class TreeClickableObject : ClickableObject
{
    public bool Fallen = false;

    bool IsBeingDraged = false;
    Camera mainCamera;

    private Animator ani;

    private float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        ani = this.GetComponent<Animator>();
        destroyTime = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (IsBeingDraged)
        //{
        //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        gameObject.transform.parent.position = hit.point;
        //    }
        //}

        if (destroyTime != -1 && destroyTime <= Time.time)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public override void BreakPower()
    {
        base.BreakPower();

        Lightning.instance.spawn(gameObject.transform.position);

        ani.SetTrigger("fall");
        destroyTime = Time.time + 2f;
    }

    public override void StartTelekinesisPower(Camera camera)
    {
        //if (Fallen)
        //{
        //    base.StartTelekinesisPower(camera);
        //    this.mainCamera = camera;
        //    IsBeingDraged = true;
        //    gameObject.GetComponent<Collider>().enabled = false;
        //}
        /* REMOVED TELEKINESIS POWER ON TREES AS IT EFFECTIVLY DID NOTHING AND WE DO NOT HAVE TIME TO MAKE IT DO SOMETHING
        if(!Fallen) 
        {
            gameObject.transform.parent.transform.rotation = Quaternion.AngleAxis(90, gameObject.transform.parent.right);
            Fallen = true;
        }
        */
    }

    public override void StopTelekinesisPower()
    {
            //base.StopTelekinesisPower();
            //IsBeingDraged = false;
            //gameObject.GetComponent<Collider>().enabled = true;
    }
}