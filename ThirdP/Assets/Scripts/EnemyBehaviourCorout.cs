using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using GPUInstance;

public class EnemyBehaviourCorout : MonoBehaviour
{
    public List<NavMeshAgent> agents = new List<NavMeshAgent>();
    public GameObject player;
    [SerializeField]
    private MyPlayer myPlayer;
    private Component animator;
    public MySolidSpawner Parent;
    //[SerializeField]
    //private float rotationSpeed;

    private void Start()
    {
        // Start a coroutine that runs continuously
        StartCoroutine(EnemyBehavior());
    }

    private IEnumerator EnemyBehavior()
    {
        
        while (true)
        {
            foreach (NavMeshAgent agent in agents)
            {
                animator = agent.GetComponentInChildren<AnimatedMesh>();
                if (animator == null)
                {
                    animator = agent.GetComponentInChildren<Animator>();
                }
                
                if (myPlayer.dead == true)
                {
                    agent.velocity = Vector3.zero;
                    if (animator is Animator)
                    {
                        Animator animatorAnim = (Animator)animator;
                        animatorAnim.SetBool("Grounded", true);
                        animatorAnim.SetFloat("Speed", 0);
                        animatorAnim.SetFloat("MotionSpeed", 0);
                    }
                    else if (animator is AnimatedMesh)
                    {
                        AnimatedMesh animatorMesh = (AnimatedMesh)animator;
                        animatorMesh.Play("Mutant Breathing Idle");
                    }
                }

                if ((player.transform.position - agent.transform.position).sqrMagnitude < Mathf.Pow(agent.stoppingDistance, 1.5f) && myPlayer.invisible == false)
                {
                    agent.enabled = false;
                    agent.GetComponent<NavMeshObstacle>().enabled = true;
                }
                else
                {
                    agent.GetComponent<NavMeshObstacle>().enabled = false;
                    agent.enabled = true;
                }

                if (agent.isOnNavMesh == true && agent.enabled == true)
                {
                    
                   
                        if (myPlayer.invisible == false&&myPlayer.dead==false)
                        {
                            agent.transform.LookAt(new Vector3(player.transform.position.x, agent.transform.position.y, player.transform.position.z));
                            agent.updatePosition = true;
                            agent.isStopped = false;
                            agent.destination = player.transform.position;
                        }

                        if (agent.velocity.magnitude >= .01f)
                        {
                            if (animator is Animator)
                            {
                                Animator animatorAnim = (Animator)animator;
                                animatorAnim.SetBool("Grounded", true);
                                animatorAnim.SetFloat("Speed", agent.velocity.magnitude);
                                animatorAnim.SetFloat("MotionSpeed", 1);
                            }
                            else if (animator is AnimatedMesh)
                            {
                                AnimatedMesh animatorMesh = (AnimatedMesh)animator;
                                animatorMesh.Play("Mutant Run");
                               
                            }
                        }
                        //if (Vector3.Distance(agent.transform.position, player.transform.position) <= 1.5 && animator is AnimatedMesh)
                        //{
                        //    agent.isStopped = true;
                           

                        //}
                        if (Vector3.Distance(agent.transform.position, player.transform.position) <= 2.2 && animator is Animator)
                        {
                            Animator animatorAnim = (Animator)animator;
                            animatorAnim.SetFloat("Speed", 0);
                            animatorAnim.SetTrigger("Attack");
                            animatorAnim.SetFloat("MotionSpeed", 0);
                            agent.isStopped = true;
                           
                        }
                    }
                
            }

            // Wait for the next frame before continuing the loop
            yield return null;
        }
    }
}

