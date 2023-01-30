

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyBehavior : MonoBehaviour
{
    public Dictionary<NavMeshAgent, NavMeshObstacle> agents = new Dictionary<NavMeshAgent, NavMeshObstacle>();
    public GameObject player;
    [SerializeField] private MyPlayer myPlayer;
    private Component animator;
    private Vector3 target;
    [Range(0.0f, 1f)] public float destUpdateRate;
    private float timer;
    public float rotationSpeed;
    private void Start()
    {
        timer = destUpdateRate;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            target = player.transform.position;
            timer = destUpdateRate;
        }
        foreach (var agent in agents)
        {
            if ((player.transform.position - agent.Key.transform.position).magnitude < agent.Key.stoppingDistance && !myPlayer.invisible)
            {
                agent.Key.transform.LookAt(new Vector3(target.x, agent.Key.transform.position.y, target.z));
                agent.Key.enabled = false;
                agent.Value.enabled = true;
                //agent.Key.GetComponentInChildren<AnimatedMesh>().Play("Mutant Breathing Idle");
                agent.Key.GetComponentInChildren<AnimatedMesh>().Play("mixamo.com");
                agent.Key.GetComponentInChildren<Silah>().HitDamage();
            }
            else
            {
                agent.Value.enabled = false;
                agent.Key.enabled = true;
            }
            if (agent.Key.isOnNavMesh && agent.Key.enabled)
            {
                animator = agent.Key.GetComponentInChildren<AnimatedMesh>();
                if (animator == null)
                {
                    animator = agent.Key.GetComponentInChildren<Animator>();
                }
                if (animator != null)
                {
                    if (!myPlayer.invisible)
                    {
                        //agent.Key.transform.LookAt(new Vector3(player.transform.position.x, agent.Key.transform.position.y, player.transform.position.z));
                        //agent.Key.transform.LookAt(new Vector3(target.x, agent.Key.transform.position.y, target.z));
                        //agent.Key.updatePosition = true;
                        agent.Key.destination = target;
                    }
                    if (agent.Key.velocity.sqrMagnitude >= .01f)
                    {
                        if (animator is Animator)
                        {
                            Animator animatorAnim = (Animator)animator;
                            animatorAnim.SetBool("Grounded", true);
                            animatorAnim.SetFloat("Speed", agent.Key.velocity.sqrMagnitude);
                            animatorAnim.SetFloat("MotionSpeed", 1);
                        }
                        else if (animator is AnimatedMesh)
                        {
                            AnimatedMesh animatorMesh = (AnimatedMesh)animator;
                            animatorMesh.Play("Mutant Run");

                        }
                    }
                    if (Vector3.Distance(agent.Key.transform.position, player.transform.position) <= 2.2 && animator is Animator && !myPlayer.invisible)
                    {
                        Animator animatorAnim = (Animator)animator;
                        animatorAnim.SetFloat("Speed", 0);
                        animatorAnim.SetTrigger("Attack");
                        animatorAnim.SetFloat("MotionSpeed", 0);
                        agent.Key.isStopped = true;
                        agent.Key.velocity = Vector3.zero;
                    }
                    if (myPlayer.dead)
                    {
                        agent.Key.isStopped = true;
                        agent.Key.updatePosition = false;
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
 
}


