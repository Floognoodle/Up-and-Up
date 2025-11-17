using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LoopBackground : MonoBehaviour
{
    public Transform player;

    private Vector3 startPos;
    private float height;
    private float offsetY = 0f;
    private float lastY = 0f;

    void Start()
    {
        startPos = transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            height = sr.bounds.size.y;
        }

        if (player != null)
        {
            lastY = player.position.y;
        }
    }

    void Update()
    {
        if (player == null) return;

        float dy = player.position.y - lastY;

        if (dy > 0f)
        {
            offsetY = offsetY + dy;

            if (height > 0f)
            {
                offsetY = offsetY % height;
            }

            Vector3 pos = transform.position;
            pos.y = startPos.y + offsetY;
            transform.position = pos;
        }

        lastY = player.position.y;
    }

    // Use when player is teleported down
    public void AdvanceForTeleport(float amount)
    {
        if (amount <= 0f) return;
        if (height <= 0f) return;

        offsetY = offsetY + amount;
        offsetY = offsetY % height;
        lastY = lastY - amount;
        Vector3 pos = transform.position;
        pos.y = startPos.y + offsetY;
        transform.position = pos;
    }
}