using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;

struct NavMeshAgentData2
{
    public Vector3 destination;
    public float speed;
    public NavMeshAgent nav;
}

struct JobSystemManager2: IJob
{
    public float deltaTime;
    public NativeArray<NavMeshAgentData2> agentData;

    public void Execute()
    {
        for (int i = 0; i < agentData.Length; i++)
        {
            var data = agentData[i];
            // Do work here with data.destination, data.speed, and deltaTime
        }
    }
}

public class NavMeshAgentSystem : MonoBehaviour
{
    public NavMeshAgent[] agents;

    private void Update()
    {
        // Convert the NavMeshAgent array to a NativeArray
        NativeArray<NavMeshAgentData2> agentData = new NativeArray<NavMeshAgentData2>(agents.Length, Allocator.TempJob);
        for (int i = 0; i < agents.Length; i++)
        {
            var agent = agents[i];
            agentData[i] = new NavMeshAgentData2
            {
                destination = agent.destination,
                speed = agent.speed
            };
        }

        // Schedule the job
        JobSystemManager2 job = new JobSystemManager2
        {
            deltaTime = Time.deltaTime,
            agentData = agentData
        };
        JobHandle handle = job.Schedule();

        // Wait for the job to complete
        handle.Complete();

        // Convert the results back to the NavMeshAgent array
        for (int i = 0; i < agents.Length; i++)
        {
            var agent = agents[i];
            var data = agentData[i];
            // Update the NavMeshAgent with the results from the job
            agent.destination = data.destination;
            agent.speed = data.speed;
        }

        // Dispose the NativeArray
        agentData.Dispose();
    }

}
