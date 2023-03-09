using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneClickableObject : ClickableObject
{

    bool IsBeingDraged = false;
    Camera mainCamera;

    public NavMeshObstacle thisObstacle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBeingDraged)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                gameObject.transform.parent.position = hit.point;
            }
            thisObstacle.enabled = false;
        } else
        {
            thisObstacle.enabled = true;
        }
    }

    public override void BreakPower()
    {
        base.BreakPower();
        Destroy(transform.parent.gameObject);
    }

    public override void StartTelekinesisPower(Camera camera)
    {
        base.StartTelekinesisPower(camera);
        this.mainCamera = camera;
        IsBeingDraged = true;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public override void StopTelekinesisPower()
    {
        base.StopTelekinesisPower();
        IsBeingDraged = false;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
