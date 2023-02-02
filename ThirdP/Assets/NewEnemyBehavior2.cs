using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyBehavior2 : MonoBehaviour
{
    private NavMeshAgentUpdateJob job;
    private JobHandle handle;
    public List<NavMeshAgent> agents;

    private void Start()
    {
        agents = new List<NavMeshAgent>();
    }

    private void LateUpdate()
    {
        job.agents = agents.ToArray();

        handle = job.Schedule(agents.Count, 10);
        handle.Complete();
    }

    public void AddAgent(NavMeshAgent agent)
    {
        agents.Add(agent);
    }

    public void RemoveAgent(NavMeshAgent agent)
    {
        agents.Remove(agent);
    }
}






