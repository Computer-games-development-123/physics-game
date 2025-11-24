using UnityEngine;

public class followPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (player == null) return;
        transform.position = player.position + offset;
    }
}
