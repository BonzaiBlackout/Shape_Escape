using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController2D : MonoBehaviour
{
   [Header("Movement")]
    public float forwardSpeed = 8f;
    public float rotationSpeed = 450f;

    [Header("Jump Settings")]
    public float shortJump = 6f;
    public float longJump = 10f;
    public float holdTime = 0.15f;

    [Header("Death")]
    public GameObject deathParticlesPrefab;
    public static int deathCount = 0;
    public float respawnDelay = 0.3f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpHoldCounter;

    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        isGrounded = true;
    }

    void Update()
    {
        // Constant forward movement
        rb.linearVelocity = new Vector2(forwardSpeed, rb.linearVelocity.y);

        // START JUMP (tap)
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, shortJump);
            isGrounded = false;
            isJumping = true;
            jumpHoldCounter = holdTime;
        }

        // HOLD JUMP (limited window)
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && isJumping)
        {
            if (jumpHoldCounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, longJump);
                jumpHoldCounter -= Time.deltaTime;
            }
        }

        // Rotation in air
        if (!isGrounded)
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground detection
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;

            float z = transform.eulerAngles.z;
            float snappedZ = Mathf.Round(z / 90f) * 90f;
            transform.eulerAngles = new Vector3(0, 0, snappedZ);
        }

        // Obstacle Detection - Death
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            //Destroy(gameObject);
            StartCoroutine(RespawnCoroutine());

            deathCount++;
        }

    }

    IEnumerator RespawnCoroutine()
    {
        // Stop movement
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Hide player (instead of deleting) on death
        GetComponent<SpriteRenderer>().enabled = false;

        // Respawn Delay
        yield return new WaitForSeconds(respawnDelay);

        // Move player back to start
        transform.position = startPosition;

        // Reset previous rotation
        transform.rotation = Quaternion.identity;

        // Unhide player
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
