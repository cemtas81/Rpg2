using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    
    public List<NavMeshAgent> agents=new List<NavMeshAgent>();
    public GameObject player;
    
    [SerializeField]
    private MyPlayer myPlayer;
    
    void Update()
    {
        foreach (NavMeshAgent agent in agents)
        {

            if (agent != null && agent.isOnNavMesh == true && agent.enabled==true)
            {
                Animator animator = agent.GetComponent<Animator>();
                if (myPlayer.invisible==false)
                {
                    //animator.SetBool("wonder", false);
                    agent.isStopped = false;
                    agent.SetDestination(player.transform.position);

                    if (agent.velocity.magnitude >= .01f)
                    {
                        
                        animator.SetBool("Grounded", true);
                        animator.SetFloat("Speed", agent.velocity.magnitude);
                        animator.SetFloat("MotionSpeed", 1);
                        agent.transform.LookAt(new Vector3(player.transform.position.x, agent.transform.position.y, player.transform.position.z));

                    }
                    if (Vector3.Distance(agent.transform.position,player.transform.position)<=1.5)
                    {
                        animator.SetTrigger("Attack");
                        animator.SetFloat("Speed", 0);
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        animator.SetFloat("MotionSpeed", 0);
                    }
                }                 
               
                if (myPlayer.invisible == true)
                {

                    animator.SetFloat("Speed", 0);
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    animator.SetFloat("MotionSpeed", 0);

                }
               

            }
           
        }
 
    }
  
}
