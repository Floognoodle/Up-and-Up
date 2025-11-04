using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 100f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
