using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{

    // Speed of the player movement
    public float speed = 10f;
    public float jumpForce = 7f;

    // Reference to the Rigidbody component
    private Rigidbody rb;

    // Movement input values
    private float movementX;
    private bool jumpPressed;
    private bool attackPressed;

    public ParticleSystem jumpParticle;
    public ParticleSystem attackParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody>();
    }

    
    void OnMove(InputValue movementValue)
    {

        // Get movement input as a Vector2
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Update movement input values
        movementX = movementVector.x;
        
        if (movementVector.y > 0.5f)
        {
            jumpPressed = true;
        }

    }


    void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            attackPressed = true;
        }
    }


    // FixedUpdate is called once per fixed frame-rate
    void FixedUpdate()
    {
        // Create a movement vector based on input
        Vector3 movement = new Vector3(movementX, 0f, 0f);

        // Apply movement to the Rigidbody2D
        rb.AddForce(movement * speed);

        if (jumpPressed)
        {
            Jump();
            JumpParticlePlay();
            jumpPressed = false;
        }

        if (attackPressed)
        {
            AttackParticlePlay();
            attackPressed = false;
        }


    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void JumpParticlePlay()
    {

        jumpParticle.transform.position = transform.position;
        jumpParticle.gameObject.SetActive(true);
        jumpParticle.Play();
    }
    void AttackParticlePlay()
    {
        if (attackParticle == null) return;

        // Offset: 0.5 right, 1.0 up
        Vector3 attackOffset = new Vector3(0.5f, 1f, 0f);
        attackParticle.transform.position = transform.position + attackOffset;

        attackParticle.gameObject.SetActive(true);
        attackParticle.Play();
    }
}

