using UnityEngine;

public class SpawnManagerSky : MonoBehaviour
{
    public GameObject[] hazardPrefabs;
    private float spawnRangeX = 20f;
    private float spawnPosZ = 20f;
    private float startDelay = 2f;
    private float spawnInterval = 1.5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnRandomHazard), startDelay, spawnInterval);
    }

    void SpawnRandomHazard()
    {
        int hazardIndex = Random.Range(0, hazardPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
        Instantiate(hazardPrefabs[hazardIndex], spawnPos, hazardPrefabs[hazardIndex].transform.rotation);
    }
}
