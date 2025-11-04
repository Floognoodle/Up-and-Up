using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 7f;

    [Header("Health (Balloons)")]
    [Range(0, 3)]
    public int balloons = 3;

    [Tooltip("Gnome art for 3, 2, 1, 0 balloons")]
    public Sprite[] balloonSprites;

    [Header("Limits")]
    public float maxUpVelocity = 20f;
    public float maxDownVelocity = -20f;
    public float bottomYLimit = -5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float horizontalInput;
    private bool jump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        UpdateBalloonSprite();
    }

    void Update()
    {
        // Check if player hits the bottom of the screen
        if (transform.position.y < bottomYLimit)
        {
            PopBalloon();
            // Teleport player back up a bit to prevent instant repeated balloon pop
            transform.position = new Vector3(transform.position.x, bottomYLimit + 2f, transform.position.z);
            rb.linearVelocity = Vector2.zero;
        }

        // Movement input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space))
            jump = true;
    }

    void FixedUpdate()
    {
        // Horizontal movement
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Jump
        if (jump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jump = false;
        }

        float clampedY = Mathf.Clamp(rb.linearVelocity.y, maxDownVelocity, maxUpVelocity);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, clampedY);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Pop 1 balloon when colliding with a hazard
        PopBalloon();
    }

    public void PopBalloon()
    {
        balloons = Mathf.Max(balloons - 1, 0);
        UpdateBalloonSprite();
    }

    public void AddBalloon()
    {
        balloons = Mathf.Min(balloons + 1, 3);
        UpdateBalloonSprite();
    }

    void UpdateBalloonSprite()
    {
        if (balloonSprites == null || balloonSprites.Length < 4)
            return;

        int spriteIndex = 3 - balloons;
        spriteRenderer.sprite = balloonSprites[spriteIndex];
    }
}
