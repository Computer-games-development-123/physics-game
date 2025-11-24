using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject platformPrefab;
    public GameObject collapsingPlatformPrefab;
    public GameObject windZoneObject;
    public Transform player;
    public Transform platformsParent;

    [Header("Spawn Settings")]
    public float startY = -3f;
    public int initialPlatforms = 15;
    public float minYGap = 2f;
    public float maxYGap = 2f;
    public float minX = -4f;
    public float maxX = 4f;
    public float distanceAhead = 15f;

    [Header("Special Platforms")]
    [Range(0f, 1f)]
    public float specialPlatformChance = 0.2f;
    public int minNormalAfterSpecial = 10;

    private int normalSinceLastSpecial = 50;
    private float highestY;

    [Header("Difficulty Levels")]
    public int platformsSpawned = 0;
    public int level = 1;
    public int level2Threshold = 50;
    public int level3Threshold = 100;

    void Start()
    {
        highestY = startY;

        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnPlatform();
        }

        if (windZoneObject != null)
        {
            windZoneObject.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null) return;

        while (highestY < player.position.y + distanceAhead)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        platformsSpawned++;
        UpdateDifficultyLevel();

        float gap = Random.Range(minYGap, maxYGap);
        highestY += gap;

        float x = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(x, highestY, 0f);

        GameObject prefabToSpawn = ChoosePlatform(spawnPos);
        if (prefabToSpawn == null)
        {
            return;
        }

        if (platformsParent != null)
        {
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, platformsParent);
        }
        else
        {
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }
    }

    void UpdateDifficultyLevel()
    {
        int previousLevel = level;

        if (platformsSpawned >= level3Threshold)
        {
            level = 3;
        }
        else if (platformsSpawned >= level2Threshold)
        {
            level = 2;
        }
        else
        {
            level = 1;
        }

        if (previousLevel != level && level == 3 && windZoneObject != null)
        {
            windZoneObject.SetActive(true);
            Debug.Log("Level 3 reached – WindZone enabled!");
        }
    }

    GameObject ChoosePlatform(Vector3 spawnPos)
    {
        GameObject prefabToSpawn = platformPrefab;

        if (level == 1)
        {
            return platformPrefab;
        }

        if (level >= 2 && collapsingPlatformPrefab != null)
        {
            if (normalSinceLastSpecial >= minNormalAfterSpecial)
            {
                if (Random.value < specialPlatformChance)
                {
                    normalSinceLastSpecial = 0;
                    return collapsingPlatformPrefab;
                }
            }

            normalSinceLastSpecial++;
        }
        return prefabToSpawn;
    }
}
