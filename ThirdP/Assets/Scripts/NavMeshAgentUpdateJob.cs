using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;

public struct NavMeshAgentUpdateJob : IJobParallelFor
{
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;
    public GameObject player;
    public MyPlayer myPlayer;
    public Component animator;
    public Vector3 target;
    public float destUpdateRate;
    public float timer;
    public float rotationSpeed;

    public void Execute(int index)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            target = player.transform.position;
            timer = destUpdateRate;
        }

        animator = agent.GetComponentInChildren<AnimatedMesh>();
        if (animator == null)
        {
            animator = agent.GetComponentInChildren<Animator>();
        }
        if ((player.transform.position - agent.transform.position).magnitude < agent.stoppingDistance && !myPlayer.invisible && animator is AnimatedMesh)
        {
            agent.transform.LookAt(new Vector3(target.x, agent.transform.position.y, target.z));
            agent.enabled = false;
            obstacle.enabled = true;
            animator.GetComponent<AnimatedMesh>().Play("mixamo.com");
            agent.GetComponentInChildren<Silah>().HitDamage();
        }
        else
        {
            obstacle.enabled = false;
            agent.enabled = true;
        }
        if (agent.isOnNavMesh && agent.enabled)
        {
            if (animator != null)
            {
                if (!myPlayer.invisible)
                {
                    agent.destination = target;
                }
                if (agent.velocity.sqrMagnitude >= .01f)
                {
                    if (animator is Animator)
                    {
                        Animator animatorAnim = (Animator)animator;
                        animatorAnim.SetBool("Grounded", true);
                        animatorAnim.SetFloat("Speed", agent.velocity.sqrMagnitude);
                        animatorAnim.SetFloat("MotionSpeed", 1);
                    }
                    else if (animator is AnimatedMesh)
                    {
                        AnimatedMesh animatorMesh = (AnimatedMesh)animator;
                        animatorMesh.Play("Mutant Run");
                    }
                }
                if (Vector3.Distance(agent.transform.position, player.transform.position) <= 2.2 && animator is Animator && !myPlayer.invisible)
                {
                    Animator animatorAnim = (Animator)animator;
                    animatorAnim.SetFloat("Speed", 0);
                    animatorAnim.SetTrigger("Attack");
                    animatorAnim.SetFloat("MotionSpeed", 0);
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                }
                if (myPlayer.dead)
                {
                    agent.isStopped = true;
                    agent.updatePosition = false;
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
            }
        }
    }
}

