using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float smoothSpeed = 3f;

    [Header("Auto Scroll Settings")]
    public bool enableAutoScroll = true;
    public float startScrollDelay = 2f;
    public float initialScrollSpeed = 0.5f;
    public float maxScrollSpeed = 3f;
    public float scrollAcceleration = 0.1f;

    private float currentScrollSpeed = 0f;
    private float scrollY;
    private float timeSinceStart = 0f;

    void Start()
    {
        scrollY = transform.position.y;
    }

    void LateUpdate()
    {
        timeSinceStart += Time.deltaTime;

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

        float desiredY = scrollY;

        if (target != null)
        {
            desiredY = Mathf.Max(scrollY, target.position.y);
        }

        Vector3 desiredPos = new Vector3(
            transform.position.x,
            desiredY,
            transform.position.z
        );

        Vector3 smoothed = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime
        );

        if (smoothed.y < transform.position.y)
        {
            smoothed.y = transform.position.y;
        }

        transform.position = smoothed;
        
        scrollY = Mathf.Max(scrollY, transform.position.y);
    }

}
