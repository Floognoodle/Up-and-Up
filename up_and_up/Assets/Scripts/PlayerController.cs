using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Health (Balloons)")]
    [Range(0, 3)]
    public int balloons = 3;
    public Sprite[] balloonSprites; // should have 4 hazards

    [Header("Limits")]
    public float maxUpVelocity = 20f;
    public float maxDownVelocity = -20f;
    public float bottomYLimit = -5f;
    [Range(0f, 1f)]
    public float topTriggerY = 0.98f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private float horizontalInput;
    private bool jumpRequested;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Sets balloon sprite art
        UpdateBalloonSprite();
    }

    void Update()
    {
        // Player controls
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Move horizontally
        Vector2 newVelocity = rb.linearVelocity;
        newVelocity.x = horizontalInput * moveSpeed;

        // Jump
        if (jumpRequested == true)
        {
            newVelocity.y = jumpForce;

            // Play jump sound
            if (audioSource != null)
            {
                if (audioSource.clip != null)
                {
                    audioSource.Play();
                }
            }

            jumpRequested = false;
        }

        // Apply velocity
        newVelocity.y = Mathf.Clamp(newVelocity.y, maxDownVelocity, maxUpVelocity);
        rb.linearVelocity = newVelocity;

        // Tp up if gnome hits bottom
        if (transform.position.y < bottomYLimit)
        {
            float teleportAmount = bottomYLimit - transform.position.y + 2f;
            Vector3 pos = transform.position;
            pos.y = pos.y + teleportAmount;
            transform.position = pos;

            Vector2 v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;
        }

        CheckTopAndLoop();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Pop a balloon if hazards are hit
        if (collision.gameObject.CompareTag("Hazard"))
        {
            PopBalloon();
        }
    }

    // Remove popped balloon & change gnome sprite
    public void PopBalloon()
    {
        balloons = Mathf.Max(0, balloons - 1);
        UpdateBalloonSprite();

        if (balloons <= 0)
        {
            PlayTimeTimer timer = FindObjectOfType<PlayTimeTimer>();
            if (timer != null)
            {
                timer.ShowLose();
            }

            Destroy(gameObject);
        }
    }

    // Add 1 balloon
    public void AddBalloon()
    {
        balloons = Mathf.Min(3, balloons + 1);
        UpdateBalloonSprite();
    }

    // Update the art to match balloons
    void UpdateBalloonSprite()
    {
        if (spriteRenderer == null) return;
        if (balloonSprites == null) return;
        if (balloonSprites.Length < 4) return;

        int index = 3 - balloons;
        index = Mathf.Clamp(index, 0, balloonSprites.Length - 1);
        spriteRenderer.sprite = balloonSprites[index];
    }

    // Map loop
    void CheckTopAndLoop()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float camZ = -cam.transform.position.z;
        Vector3 topViewportPoint = new Vector3(0f, topTriggerY, camZ);
        Vector3 bottomViewportPoint = new Vector3(0f, 0f, camZ);

        float topWorldY = cam.ViewportToWorldPoint(topViewportPoint).y;
        float bottomWorldY = cam.ViewportToWorldPoint(bottomViewportPoint).y;
        float screenHeight = topWorldY - bottomWorldY;

        if (screenHeight <= 0f) return;

        if (transform.position.y > topWorldY)
        {
            LoopBackground[] backgrounds = FindObjectsOfType<LoopBackground>();
            for (int i = 0; i < backgrounds.Length; i++)
            {
                LoopBackground bg = backgrounds[i];
                if (bg != null)
                {
                    bg.AdvanceForTeleport(screenHeight);
                }
            }

            Vector3 pos = transform.position;
            pos.y = pos.y - screenHeight;
            transform.position = pos;

            Vector2 v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;

            // Destroy all hazards
            GameObject[] hazards = GameObject.FindGameObjectsWithTag("Hazard");
            for (int i = 0; i < hazards.Length; i++)
            {
                if (hazards[i] != null)
                {
                    Destroy(hazards[i]);
                }
            }
        }
    }
}
