using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

 
    public Transform enemy;
   

    public float speed = 10f;
    public float jumpForce = 7f;

    public float jumpCooldown = 0.5f;
    public float attackCooldown = 1.5f;

    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    public float attackRange = 1.5f;
    public float attackDamage = 10f;
    public bool isAttacking = false;



    public ParticleSystem jumpParticle;

    public AudioSource audioMovement;
    public AudioSource audioJump;
    public AudioSource audioAttack;

    private Animator anim;
    private Rigidbody rb;

    private float movementX;
    private bool jumpPressed;
    private bool attackPressed;
    private bool isDead = false;

    private float lastJumpTime = 0f;
    private float lastAttackTime = 0f;

   
    void Start()
    {
        // get components
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
       
        // set starting health
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    void OnMove(InputValue movementValue)
    {
        // get movement input
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;

        // check for jump input
        if (movementVector.y > 0.5f)
            jumpPressed = true;
    }

    void OnAttack(InputValue value)
    {
        // check for attack input
        if (value.isPressed)
            attackPressed = true;
    }

    void Update()
    {
        // update walking animation
        if (!isAttacking)
            anim.SetBool("isWalking", Mathf.Abs(movementX) > 0.01f);
        else
            anim.SetBool("isWalking", false);

        // play or stop movement audio
        if (Mathf.Abs(movementX) > 0.01f)
        {
            if (audioMovement != null && !audioMovement.isPlaying)
                audioMovement.Play();
        }
        else
        {
            if (audioMovement != null && audioMovement.isPlaying)
                audioMovement.Stop();
        }

        // flip player direction
        if (movementX > 0.01f)
            transform.localScale = Vector3.one;
        else if (movementX < -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    void FixedUpdate()
    {
        //// apply movement force
        rb.AddForce(new Vector3(movementX, 0f, 0f) * speed);

        //// apply movement
        //rb.linearVelocity = new Vector3(
        //    movementX * speed,
        //    rb.linearVelocity.y,
        //    0f
        //);

        // handle jump with cooldown
        if (jumpPressed)
        {
            if (Time.time >= lastJumpTime + jumpCooldown)
            {
                Jump();
                PlayJumpParticle();

                if (audioJump != null)
                    audioJump.Play();

                anim.SetTrigger("Jump");

                lastJumpTime = Time.time;
            }

            jumpPressed = false;
        }

        // handle attack with cooldown
        if (attackPressed)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();

                if (audioAttack != null)
                    audioAttack.Play();

                lastAttackTime = Time.time;
            }

            attackPressed = false;
            isAttacking = false;
        }
    }

    void Jump()
    {
        // reset vertical velocity and apply jump force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void Attack()
    {
        // trigger attack animation
        anim.SetTrigger("Attack");
        isAttacking = true;
       

        if (enemy == null)
            return;

        float distance = Vector3.Distance(transform.position, enemy.position);

        if (distance <= attackRange)
        {
            EnemyBehaviour enemyHealth = enemy.GetComponent<EnemyBehaviour>();

            if (enemyHealth != null)
            {
                // deal damage and apply knockback
                enemyHealth.TakeDamage(attackDamage);
                enemyHealth.ApplyKnockback(transform.position);
            }
        }
    }


    void PlayJumpParticle()
    {
        // play jump particle if assigned
        if (jumpParticle == null) return;

        jumpParticle.transform.position = transform.position;
        jumpParticle.Play();
    }

    public void TakeDamage(float amount)
    {
        // reduce health
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        UpdateHealthSlider();
        Debug.Log("player took damage: " + currentHealth);

        // check death
        if (currentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        // increase health
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        UpdateHealthSlider();
    }

    public void UpdateHealthSlider()
    {
        // update ui slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Die()
    {
        
        GameManager.instance.PlayerWonRound();

        //if (isDead) return;

        //isDead = true;

        //// play death animation
        //anim.SetTrigger("Die");

        //// stop movement
        //rb.linearVelocity = Vector3.zero;

        //// stop audio
        //if (audioMovement != null)
        //    audioMovement.Stop();
    }

    //public void DeathAnimationEnd()
    //{
    //    if (GameManager.instance != null)
    //        GameManager.instance.EnemyWonRound();
    //}

    void OnTriggerEnter(Collider other)
    {
        // die if falling into void
        if (other.CompareTag("Void"))
            Die();
    }
}