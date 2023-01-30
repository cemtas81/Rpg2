using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flocking : MonoBehaviour
{
    public List<NavMeshAgent> agents = new();
    public float minDistance = 3.0f;
    public NewEnemyBehavior newEnemyBehavior;
    public Transform player;
    public float smoothness;
    public MyPlayer myPlayer;
    private Component animator;
    private NavMeshPath path;
    void Update()
    {
        Vector3 targetPos = player.position;
        //agents = newEnemyBehavior.agents;

        for (int i = 0; i < agents.Count; i++)
        {
            if ((player.transform.position - agents[i].transform.position).sqrMagnitude < Mathf.Pow(agents[i].stoppingDistance, 1f) && !myPlayer.invisible)
            {
                agents[i].enabled = false;
                agents[i].GetComponent<NavMeshObstacle>().enabled = true;
            }
            else
            {
                agents[i].GetComponent<NavMeshObstacle>().enabled = false;
                agents[i].enabled = true;

            }
            if (agents[i].isOnNavMesh && agents[i].enabled && !myPlayer.dead && !myPlayer.invisible)
            {
                animator = agents[i].GetComponentInChildren<AnimatedMesh>();
                agents[i].transform.LookAt(new Vector3(player.transform.position.x, agents[i].transform.position.y, player.transform.position.z));
                if (animator == null)
                {
                    animator = agents[i].GetComponentInChildren<Animator>();
                }
                agents[i].updatePosition = true;
                agents[i].isStopped = false;
                agents[i].SetDestination(targetPos);
                if (agents[i].remainingDistance == 0 &&animator!=null)
                {
                    agents[i].SetDestination(targetPos);
                }
                if (agents[i].velocity.magnitude >= .01f)
                {
                    if (animator is Animator)
                    {
                        Animator animatorAnim = (Animator)animator;
                        animatorAnim.SetBool("Grounded", true);
                        animatorAnim.SetFloat("Speed", agents[i].velocity.magnitude);
                        animatorAnim.SetFloat("MotionSpeed", 1);
                    }
                    else if (animator is AnimatedMesh)
                    {
                        AnimatedMesh animatorMesh = (AnimatedMesh)animator;
                        animatorMesh.Play("Mutant Run");

                    }

                }
                if (Vector3.Distance(agents[i].transform.position, player.transform.position) <= 2.2 && animator is Animator && !myPlayer.invisible)
                {
                    Animator animatorAnim = (Animator)animator;
                    animatorAnim.SetFloat("Speed", 0);
                    animatorAnim.SetTrigger("Attack");
                    animatorAnim.SetFloat("MotionSpeed", 0);
                    agents[i].isStopped = true;
                    agents[i].velocity = Vector3.zero;
                }
                if (myPlayer.dead)
                {
                    agents[i].isStopped = true;
                    agents[i].updatePosition = false;
                    if (animator is Animator)
                    {
                        Animator animatorAnim = (Animator)animator;
                        animatorAnim.SetFloat("Speed", 0);
                        animatorAnim.SetBool("Grounded", false);
                        animatorAnim.SetFloat("MotionSpeed", 0);
                    }
                    else if (animator is AnimatedMesh)
                    {
                        AnimatedMesh animatorMesh = (AnimatedMesh)animator;
                        animatorMesh.Play("Mutant Breathing Idle");
                    }
                }
                for (int j = 0; j < agents.Count; j++)
                {
                    if (i != j)
                    {
                        float distanceToAgent = Vector3.Distance(agents[i].transform.position, agents[j].transform.position);
                        if (distanceToAgent <= minDistance)
                        {
                            Vector3 avoidDirection = (agents[i].transform.position - agents[j].transform.position).normalized;
                            agents[i].SetDestination(agents[i].transform.position + avoidDirection * (minDistance - distanceToAgent));

                        }
                        float distanceToPlayer = Vector3.Distance(agents[i].transform.position, player.transform.position);
                        if (distanceToPlayer <= minDistance)
                        {
                            Vector3 avoidDirection = (agents[i].transform.position - player.transform.position).normalized;
                            agents[i].SetDestination(agents[i].transform.position + avoidDirection * (minDistance - distanceToPlayer));

                        }
                    }
                }
            }
        }
    }
}
