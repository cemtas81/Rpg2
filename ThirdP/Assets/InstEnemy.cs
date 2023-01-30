using FSG.MeshAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AnimationInstancing;
public class InstEnemy : MonoBehaviour
{
    public List<NavMeshAgent> agents = new List<NavMeshAgent>();
    public GameObject player;
    [SerializeField]
    private MyPlayer myPlayer;

    void Update()
    {

        foreach (NavMeshAgent agent in agents)
        {

            if (agent != null && agent.isOnNavMesh == true && agent.enabled == true)
            {
                //Animator animator = agent.GetComponentInChildren<Animator>();

                if (myPlayer.invisible == false)
                {

                    agent.updatePosition = true;
                    //animator.SetBool("wonder", false);
                    agent.isStopped = false;
                    agent.SetDestination(player.transform.position);
                    agent.transform.LookAt(new Vector3(player.transform.position.x, agent.transform.position.y, player.transform.position.z));

                    //if (agent.velocity.magnitude >= .01f && agent.velocity.magnitude < 2f)
                    //{
                    //    animator.Play("Walk_N");
                    //}
                    //if (agent.velocity.magnitude >= 2)
                    //{
                    //    animator.Play("Run_N");
                    //}
                    //if (Vector3.Distance(agent.transform.position, player.transform.position) <= 1.5)
                    //{
                    //    animator.Play("mixamo.com");
                    //    agent.isStopped = true;
                    //    agent.velocity = Vector3.zero;

                    //}

                }

                //if (myPlayer.invisible == true)
                //{

                //    animator.Play("Idle");
                //    agent.isStopped = true;
                //    agent.updatePosition = false;

                //}

            }

        }

    }
}
