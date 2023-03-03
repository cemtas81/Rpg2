using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        changeTillValidTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= 0.2)
        {
            changeTillValidTarget();
        }
    }

    //Change till valid target will keep trying to change the target randomly untill it finds a target it is able to do
    public void changeTillValidTarget()
    {
        if (!changeTarget())
        {
            changeTillValidTarget();
        }


    }

    //Change target will pick a random target to go to. Returns true if path is valid and false if it can not get there
    public bool changeTarget()
    {
        return changeTarget(TargetHub.instance.getRandom().gameObject.transform.position);
    }

    //Change target vector3 will set the destination to a specific location. Returns true if path is valid and false if it can not get there
    public bool changeTarget(Vector3 newDestination)
    {
        NavMeshPath path = new NavMeshPath();
        agent.destination = newDestination;
        agent.CalculatePath(newDestination, path);

        if(path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        } else
        {
            return true;
        }
        
    }
}
