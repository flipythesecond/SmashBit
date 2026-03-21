using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 7f;

    public int maxHealth = 10;
    public int currentHealth;
    public Slider healthSlider;

    private Animator anim;
    private Rigidbody rb;

    private float movementX;
    private bool jumpPressed;
    private bool attackPressed;

    public ParticleSystem jumpParticle;
    public ParticleSystem attackParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
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
    void Update()
    {
        // Walking animation
        if (anim != null)
        {
            bool isWalking = Mathf.Abs(movementX) > 0.01f;
            anim.SetBool("isWalking", isWalking);
        }

        // Flip player left/right
        if (movementX > 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (movementX < -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0f, 0f);
        rb.AddForce(movement * speed);

        if (jumpPressed)
        {
            Jump();
            JumpParticlePlay();

            if (anim != null)
            {
                anim.SetTrigger("Jump");
            }

            jumpPressed = false;
        }

        if (attackPressed)
        {
            AttackParticlePlay();

            if (anim != null)
            {
                anim.SetTrigger("Attack");
            }

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
        if (jumpParticle == null) return;

        jumpParticle.transform.position = transform.position;
        jumpParticle.gameObject.SetActive(true);
        jumpParticle.Play();
    }
    void AttackParticlePlay()
    {
        if (attackParticle == null) return;

        Vector3 attackOffset;

        // Put attack particle in front of the player based on facing direction
        if (transform.localScale.x > 0)
        {
            attackOffset = new Vector3(0.5f, 1f, 0f);
        }
        else
        {
            attackOffset = new Vector3(-0.5f, 1f, 0f);
        }

        attackParticle.transform.position = transform.position + attackOffset;
        attackParticle.gameObject.SetActive(true);
        attackParticle.Play();
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthSlider();
        Debug.Log("Player took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthSlider();
    }
    void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
    void Die()
    {
        Debug.Log("Player died");
        gameObject.SetActive(false);
    }
}