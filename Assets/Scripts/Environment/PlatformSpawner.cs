using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject platformPrefab;              // פלטפורמה רגילה
    public GameObject collapsingPlatformPrefab;    // פלטפורמה קורסה
    public GameObject windZoneObject;              // אובייקט רוח בסצנה (ילד של המצלמה)
    public Transform player;                       // השחקן
    public Transform platformsParent;              // הורה לכל הפלטפורמות (לא חובה)

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
    public float specialPlatformChance = 0.2f;     // הסתברות בסיסית לפלטפורמה מיוחדת (קורסה)
    public int minNormalAfterSpecial = 20;         // כמה פלטפורמות רגילות לפחות אחרי מיוחדת

    private int normalSinceLastSpecial = 100;      // מונה פלטפורמות רגילות מאז המיוחדת האחרונה
    private float highestY;

    [Header("Difficulty Levels")]
    public int platformsSpawned = 0;               // כמה פלטפורמות יצרנו עד עכשיו
    public int level = 1;                          // דרגת קושי נוכחית

    public int level2Threshold = 50;               // אחרי כמה פלטפורמות עוברים לשלב 2
    public int level3Threshold = 100;              // אחרי כמה פלטפורמות עוברים לשלב 3

    void Start()
    {
        highestY = startY;

        // יצירה התחלתית של פלטפורמות
        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnPlatform();
        }

        // לוודא ש-WindZone כבוי בתחילת המשחק (אם לא כיבית ידנית)
        if (windZoneObject != null)
        {
            windZoneObject.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null) return;

        // כל עוד אין מספיק פלטפורמות מעל השחקן – נוסיף חדשות
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

        // אם הרגע עלינו לשלב 3 – מפעילים את איזור הרוח פעם אחת
        if (previousLevel != level && level == 3 && windZoneObject != null)
        {
            windZoneObject.SetActive(true);
            Debug.Log("Level 3 reached – WindZone enabled!");
        }
    }

    GameObject ChoosePlatform(Vector3 spawnPos)
    {
        // ברירת מחדל – פלטפורמה רגילה
        GameObject prefabToSpawn = platformPrefab;

        // -------------------------
        // שלב 1 – רק פלטפורמות רגילות
        // -------------------------
        if (level == 1)
        {
            return platformPrefab;
        }

        // -------------------------
        // שלב 2 ומעלה – מוסיפים פלטפורמות קורסות לפי הסתברות
        // -------------------------
        if (level >= 2 && collapsingPlatformPrefab != null)
        {
            // לוודא שיש לפחות X פלטפורמות רגילות בין מיוחדות
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

        // אם לא יצרנו מיוחדת – יוצרת רגילה
        return prefabToSpawn;
    }
}
