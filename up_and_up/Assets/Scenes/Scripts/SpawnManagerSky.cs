using UnityEngine;

public class SpawnManagerSky : MonoBehaviour
{
    public GameObject[] cloudPrefabs;
    private float spawnRangeX = 20f;
    private float spawnPosZ = 20f;
    private float startDelay = 2f;
    private float spawnInterval = 1.5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnRandomCloud), startDelay, spawnInterval);
    }

    void SpawnRandomCloud()
    {
        int cloudIndex = Random.Range(0, cloudPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
        Instantiate(cloudPrefabs[cloudIndex], spawnPos, cloudPrefabs[cloudIndex].transform.rotation);
    }
}
