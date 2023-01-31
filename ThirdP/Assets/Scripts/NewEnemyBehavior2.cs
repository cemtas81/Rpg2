using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class NewEnemyBehavior2 : MonoBehaviour
{
    public List<NavMeshAgent> agents = new List<NavMeshAgent>();
    public GameObject player;
    [SerializeField]
    private MyPlayer myPlayer;
    public MySolidSpawner Parent;
    private JobHandle jobHandle;
    //private NavMeshAgent[] nativeAgents;
    private Allocator allocator = Allocator.Persistent;
    private NativeArray<int> nativeAgentsIndices;

    void Update()
    {
        if (nativeAgentsIndices.IsCreated)
        {
            nativeAgentsIndices.Dispose();
        }
        nativeAgentsIndices = new NativeArray<int>(agents.Count, Allocator.TempJob);

        for (int i = 0; i < agents.Count; i++)
        {
            nativeAgentsIndices[i] = i;
        }

        NavMeshAgentUpdateJob job = new NavMeshAgentUpdateJob();
        job.agentsIndices = nativeAgentsIndices;
        job.agentsList = agents;
        job.player = player;
        job.myPlayer = myPlayer;
        jobHandle = job.Schedule(nativeAgentsIndices.Length, 32);
    }


    void OnDestroy()
    {
        jobHandle.Complete();
        if (nativeAgentsIndices.IsCreated)
        {
            nativeAgentsIndices.Dispose();
        }
    }

    public struct NavMeshAgentUpdateJob : IJobParallelFor
{
    public NavMeshAgent[] agents;
    public GameObject player;
    public MyPlayer myPlayer;
        internal NativeArray<int> agentsIndices;
        internal List<NavMeshAgent> agentsList;

        public void Execute(int index)
    {
        NavMeshAgent agent = agents[index];
        if ((player.transform.position - agents[index].transform.position).sqrMagnitude < Mathf.Pow(agents[index].stoppingDistance, 1f) && !myPlayer.invisible)
        {
            agents[index].enabled = false;
            agents[index].GetComponent<NavMeshObstacle>().enabled = true;
        }
        else
        {
            agents[index].GetComponent<NavMeshObstacle>().enabled = false;
            agents[index].enabled = true;
        }
        if (agents[index].isOnNavMesh && agents[index].enabled)
        {
            Component animator = agents[index].GetComponentInChildren<AnimatedMesh>();
            if (animator == null)
            {
                animator = agents[index].GetComponentInChildren<Animator>();
            }
            if (animator != null)
            {
                if (!myPlayer.invisible)
                {
                    agents[index].transform.LookAt(new Vector3(player.transform.position.x, agents[index].transform.position.y, player.transform.position.z));
                    agents[index].updatePosition = true;
                    agents[index].destination = player.transform.position;
                }
                if (agents[index].velocity.sqrMagnitude >= .01f)
                {
                    if (animator is Animator)
                    {
                        Animator animatorAnim = (Animator)animator;
                        animatorAnim.SetBool("Grounded", true);
                        animatorAnim.SetFloat("Speed", agents[index].velocity.sqrMagnitude);
                        animatorAnim.SetFloat("MotionSpeed", 1);
                    }
                    else if (animator is AnimatedMesh)
                    {
                        AnimatedMesh animatorMesh = (AnimatedMesh)animator;
                        animatorMesh.Play("Mutant Run");
                    }
                }
                if (Vector3.Distance(agents[index].transform.position, player.transform.position) <= 2.2 && animator is Animator && !myPlayer.invisible)
                {
                    Animator animatorAnim = (Animator)animator;
                    animatorAnim.SetFloat("Speed", 0);
                    animatorAnim.SetTrigger("Attack");
                    animatorAnim.SetFloat("MotionSpeed", 0);
                    agents[index].isStopped = true;
                    agents[index].velocity = Vector3.zero;
                }
                if (myPlayer.dead)
                {
                    agents[index].isStopped = true;
                    agents[index].updatePosition = false;
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






