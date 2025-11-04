using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 startPos;
    private float repeatHeight;
    void Start()
    {
        startPos = transform.position;
        repeatHeight = GetComponent<BoxCollider2D>().size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < startPos.y - repeatHeight)
        {
            transform.position = startPos;
        }
    }
}
