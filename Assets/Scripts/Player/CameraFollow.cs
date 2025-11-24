using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;          // השחקן

    [Header("Follow Settings")]
    public float smoothSpeed = 3f;    // כמה חלק המצלמה זזה אחרי המטרה

    [Header("Auto Scroll Settings")]
    public bool enableAutoScroll = true;
    public float startScrollDelay = 2f;      // כמה זמן אחרי תחילת המשחק להתחיל לזוז
    public float initialScrollSpeed = 0.5f;  // מהירות התחלתית של הגלילה
    public float maxScrollSpeed = 3f;        // מהירות מקסימלית של הגלילה
    public float scrollAcceleration = 0.1f;  // כמה מהר המהירות עולה (יחידות לשנייה)

    private float currentScrollSpeed = 0f;
    private float scrollY;            // הגובה המינימלי של המצלמה (ה"גלילה")
    private float timeSinceStart = 0f;

    void Start()
    {
        // מתחילים את הגלילה מגובה המצלמה הנוכחי
        scrollY = transform.position.y;
    }

    void LateUpdate()
    {
        timeSinceStart += Time.deltaTime;

        // 1. גלילה אוטומטית כלפי מעלה
        if (enableAutoScroll && timeSinceStart >= startScrollDelay)
        {
            if (currentScrollSpeed < initialScrollSpeed)
            {
                currentScrollSpeed = initialScrollSpeed;
            }

            currentScrollSpeed = Mathf.MoveTowards(
                currentScrollSpeed,
                maxScrollSpeed,
                scrollAcceleration * Time.deltaTime
            );

            scrollY += currentScrollSpeed * Time.deltaTime;
        }

        // 2. מחשבים את הגובה הרצוי של המצלמה
        float desiredY = scrollY;

        if (target != null)
        {
            // המצלמה לא תרד מתחת לבייסליין של הגלילה,
            // אבל אם השחקן גבוה יותר – נעלה אל השחקן
            desiredY = Mathf.Max(scrollY, target.position.y);
        }

        Vector3 desiredPos = new Vector3(
            transform.position.x,
            desiredY,
            transform.position.z
        );

        // 3. smoothing לתנועה חלקה
        Vector3 smoothed = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime
        );

        // 4. לא נותנים למצלמה לרדת אף פעם
        if (smoothed.y < transform.position.y)
        {
            smoothed.y = transform.position.y;
        }

        // 5. מזיזים את המצלמה למיקום הסופי
        transform.position = smoothed;

        // 6. מעדכנים את scrollY כדי שתמיד ישקף את הגובה המקסימלי של המצלמה
        //    כלומר – אם השחקן גרם למצלמה לעלות יותר גבוה,
        //    הבייסליין של הגלילה האוטומטית "מדלג" לגובה החדש
        scrollY = Mathf.Max(scrollY, transform.position.y);
    }

}
