using UnityEngine;

public class SpawnManagerSky : MonoBehaviour
{
    public GameObject[] hazardPrefabs;

    public float spawnRangeX = 20f;
    public float spawnPosZ = 20f;
    public float startDelay = 2f;
    public float spawnInterval = 1.5f;

    public int maxHazards = 4;
    private bool minutePassed = false;

    void Start()
    {
        InvokeRepeating("SpawnRandomHazard", startDelay, spawnInterval);
    }

    void SpawnRandomHazard()
    {
        // Count hazards
        GameObject[] existingHazards = GameObject.FindGameObjectsWithTag("Hazard");
        if (existingHazards.Length >= maxHazards) return;

        if (hazardPrefabs == null) return;
        if (hazardPrefabs.Length == 0) return;

        int[] allowedIndices = GetAllowedHazardIndices();
        if (allowedIndices == null) return;
        if (allowedIndices.Length == 0) return;

        int pickIndex = Random.Range(0, allowedIndices.Length);
        pickIndex = allowedIndices[pickIndex];

        // Prevent duplicates
        if (existingHazards.Length == maxHazards - 1)
        {
            int sameTypeCount = 0;
            string pickName = hazardPrefabs[pickIndex].name;
            for (int i = 0; i < existingHazards.Length; i++)
            {
                GameObject h = existingHazards[i];
                if (h != null && h.name.Contains(pickName))
                {
                    sameTypeCount++;
                }
            }

            if (sameTypeCount >= 1) return;
        }

        // Spawn on the right side only
        float x = Random.Range(0f, spawnRangeX);
        Vector3 spawnPos = new Vector3(x, 0f, spawnPosZ);

        GameObject spawned = Instantiate(hazardPrefabs[pickIndex], spawnPos, hazardPrefabs[pickIndex].transform.rotation);
        if (spawned != null)
        {
            spawned.tag = "Hazard";
        }
    }

    private int[] GetAllowedHazardIndices()
    {
        int n = hazardPrefabs == null ? 0 : hazardPrefabs.Length;
        if (n == 0) return new int[0];

        if (n >= 4)
        {
            if (!minutePassed)
            {
                return new int[] { 0, 1 };
            }
            else
            {
                return new int[] { 2, 3 };
            }
        }

        int half = Mathf.Max(1, n / 2);

        if (!minutePassed)
        {
            int len = Mathf.Min(half, n);
            int[] arr = new int[len];
            for (int i = 0; i < len; i++) arr[i] = i;
            return arr;
        }
        else
        {
            int start = half;
            int len = n - start;
            if (len <= 0) len = 1;
            int[] arr = new int[len];
            for (int i = 0; i < len; i++) arr[i] = start + i;
            return arr;
        }
    }

    public void SetMinutePassed(bool minuteIsPassed, bool clearOldTypes = true)
    {
        if (minuteIsPassed == minutePassed) return;

        minutePassed = minuteIsPassed;

        if (!clearOldTypes) return;

        if (hazardPrefabs == null) return;
        int n = hazardPrefabs.Length;
        if (n == 0) return;

        int half = Mathf.Max(1, n / 2);

        int start = 0;
        int end = 0;
        if (minutePassed)
        {
            start = 0;
            end = Mathf.Min(half, n);
        }
        else
        {
            start = half;
            end = n;
        }

        for (int i = start; i < end; i++)
        {
            string prefabName = hazardPrefabs[i].name;
            GameObject[] hazards = GameObject.FindGameObjectsWithTag("Hazard");
            for (int j = 0; j < hazards.Length; j++)
            {
                GameObject h = hazards[j];
                if (h != null && h.name.Contains(prefabName))
                {
                    Destroy(h);
                }
            }
        }
    }
}