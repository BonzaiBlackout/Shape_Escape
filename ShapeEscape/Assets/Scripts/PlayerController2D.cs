using UnityEngine;
using UnityEngine.SceneManagement;

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

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpHoldCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            Destroy(gameObject);

            deathCount++;

            Debug.Log("Hit: " + collision.gameObject.name);
        }
    }
}
