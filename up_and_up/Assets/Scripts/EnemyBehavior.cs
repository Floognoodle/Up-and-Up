using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 100f;

    void Update()
    {
        // Move left
        Vector3 move = Vector3.left * speed * Time.deltaTime;
        transform.Translate(move);
    }
}