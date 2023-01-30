using UnityEngine;

public class MyGpuInstancing : MonoBehaviour
{
    public GameObject prefab;
    public int instanceCount = 1000;

    private GameObject[] instances;
    private Matrix4x4[] matrices;
    private ComputeBuffer matricesBuffer;

    void Start()
    {
        // Create a buffer to hold the transform matrices for each instance
        matrices = new Matrix4x4[instanceCount];
        matricesBuffer = new ComputeBuffer(instanceCount, 64);
        UpdateMatrices();

        // Create an array to store the instances of the prefab
        instances = new GameObject[instanceCount];

        // Create a new instance of the prefab for each desired instance
        for (int i = 0; i < instanceCount; i++)
        {
            GameObject instance = Instantiate(prefab);
            instances[i] = instance;
        }

        // Set the buffer as a global property for the prefab's material
        prefab.GetComponent<MeshRenderer>().material.SetBuffer("matrices", matricesBuffer);
    }

    void Update()
    {
        UpdateMatrices();
        matricesBuffer.SetData(matrices);
    }

    void UpdateMatrices()
    {
        // Generate a random transformation matrix for each instance
        for (int i = 0; i < instanceCount; i++)
        {
            matrices[i] = Matrix4x4.TRS(
                Random.insideUnitSphere * 10,
                Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360),
                Vector3.one
            );
        }
    }

    void OnRenderObject()
    {
        // Draw the prefab's mesh with instancing
        prefab.GetComponent<MeshRenderer>().material.SetPass(0);
        Graphics.DrawMeshInstanced(prefab.GetComponent<MeshFilter>().mesh, 0, prefab.GetComponent<MeshRenderer>().material, matrices, instanceCount);
    }

    void OnDisable()
    {
        // Release the buffer when the script is disabled
        matricesBuffer.Release();
    }
}

