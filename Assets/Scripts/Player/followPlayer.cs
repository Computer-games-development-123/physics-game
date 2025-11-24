using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // השחקן
    public float smoothSpeed = 3f;
    private float highestY;

    void Start()
    {
        highestY = transform.position.y-10;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // אם השחקן גבוה יותר מהמצלמה – עולים למעלה
        if (target.position.y > highestY)
        {
            highestY = target.position.y;

            Vector3 desiredPos = new Vector3(
                transform.position.x,
                highestY,
                transform.position.z
            );

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPos,
                smoothSpeed * Time.deltaTime
            );
        }
    }
}
