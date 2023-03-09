using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    public List<MonsterTargetLocation> targets;

    private NavMeshSurface surface;
    public NavMeshAgent agent;

    //bool to stop it's standard rutine if the character is near
    private bool chasing = false;
    private bool wasChasing = false;

    //This can be changed to make the monster see further
    public float maxRange = 50;

    private GameObject player;

    //There should be an empty game object that contains all the targets for each specific monster
    public GameObject targetsContainer;

    //speed
    private float speed;
    private Vector3 lastPosition;

    public Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in targetsContainer.transform)
        {
            if (child.GetComponent<MonsterTargetLocation>() != null)
            {
                targets.Add(child.GetComponent<MonsterTargetLocation>());
            }
        }

        player = GameObject.FindGameObjectWithTag("Player");
        surface = TargetHub.instance.surface;
    }

    // Update is called once per frame
    void Update()
    {
        if (!chasing)
        {
            if (agent.remainingDistance <= 0.2)
            {
                changeTillValidTarget();
            }
        } else
        {
            changeTarget(player.transform.position);
        }

        //RaycastHit hit;
        if (Vector3.Distance(transform.position, player.transform.position) < maxRange)
        {
            /*
             * For some unknown reason this only works sometimes. It'll work perfectly for a while but then all of a sudden decide to stop
             * for no good reason whatsoever. I have spent too long trying to fix this so I am cutting the eye contact thing and making it only 
             * being close.
             * 
            Debug.Log("Close");
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, maxRange))
            {

                Debug.Log("ray");
                if (hit.transform == player.transform)
                {
                    Debug.Log("attack");
                    //Player is in range and can be seen.
                    chasing = true;
                }
                else
                {
                    Debug.Log("No attacking");
                    chasing = false;
                }
            }
            */
            chasing = true;
            wasChasing = true;
        } else
        {
            chasing = false;
            if (wasChasing)
            {
                changeTillValidTarget();
                wasChasing = false;
            }
        }

        //Determine if the enemy is close enough to attack.
        //Super bad way of determining collisions but for this type of game and it being a jam this is totally sufficiant.
        if (Vector3.Distance(transform.position, player.transform.position) < 1.2f)
        {
            player.GetComponent<MainCharacterScript>().hit();
            GameObject.Destroy(this.gameObject);
        }

        //Speed
        Vector3 curMove = transform.position - lastPosition;
        speed = curMove.magnitude / Time.deltaTime;
        lastPosition = transform.position;

        ani.SetFloat("speed", speed);
    }

    #region Targeting

    public void changeTillValidTarget()
    {
        if (!changeTarget())
        {
            changeTillValidTarget();
        }
    }

    public MonsterTargetLocation getRandom()
    {
        return targets[Random.Range(0, targets.Count)];
    }

    //Change target will pick a random target to go to. Returns true if path is valid and false if it can not get there
    public bool changeTarget()
    {
        return changeTarget(getRandom().transform.position);
    }

    //Change target vector3 will set the destination to a specific location. Returns true if path is valid and false if it can not get there
    public bool changeTarget(Vector3 newDestination)
    {
        NavMeshPath path = new NavMeshPath();
        agent.destination = newDestination;
        agent.CalculatePath(newDestination, path);

        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    #endregion
}
