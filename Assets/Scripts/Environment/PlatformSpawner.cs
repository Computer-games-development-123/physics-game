using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject platformPrefab;        // פלטפורמה רגילה
    public GameObject springPlatformPrefab;  // פלטפורמת קפיץ
    public Transform player;                 // השחקן
    public Transform platformsParent;        // הורה לכל הפלטפורמות (לא חובה)

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
    public float springPlatformChance = 0.2f;    // הסתברות בסיסית לקפיץ
    public int minNormalAfterSpring = 20;        // כמה פלטפורמות רגילות לפחות אחרי קפיץ

    // מונה כמה פלטפורמות רגילות היו מאז הקפיץ האחרון
    private int normalSinceLastSpring = 100;

    private float highestY;

    void Start()
    {
        highestY = startY;

        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnPlatform();
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
        float gap = Random.Range(minYGap, maxYGap);
        highestY += gap;

        float x = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(x, highestY, 0f);

        // ברירת מחדל – פלטפורמה רגילה
        GameObject prefabToSpawn = platformPrefab;

        // ננסה ליצור קפיץ רק אם:
        // 1. יש לנו prefab של קפיץ
        // 2. עברו מספיק פלטפורמות רגילות מאז הקפיץ האחרון
        if (springPlatformPrefab != null && normalSinceLastSpring >= minNormalAfterSpring)
        {
            float r = Random.Range(0f, 1f);
            if (r < springPlatformChance)
            {
                prefabToSpawn = springPlatformPrefab;
                normalSinceLastSpring = 0; // מאפסים – עכשיו בדיוק היה קפיץ
            }
            else
            {
                normalSinceLastSpring++;   // עוד פלטפורמה רגילה
            }
        }
        else
        {
            // עדיין בתקופה שבה חייבים רגילות בלבד
            normalSinceLastSpring++;
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
}
