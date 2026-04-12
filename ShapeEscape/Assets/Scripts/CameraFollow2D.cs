using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;
    public float offsetX = 5f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 pos = transform.position;
        pos.x = player.position.x + offsetX;

        transform.position = pos;
    }
}