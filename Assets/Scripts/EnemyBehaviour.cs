using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    // settings for enemy behavior
    public Transform player;
    public float detectRange = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public float attackDamage = 10f;
    // jump settings
    public float jumpForce = 7f;
    public float jumpCooldown = 3f;
    public float playerAboveAmount = 1f;
    // health settings
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    // knockback settings
    public float knockBackForce = 4f;
    public float knockbackUpForce = 2f;
    public float knockbackDuration = 1f;
    public float attackDuration = 1.5f;

    // audio sources
    public AudioSource audioMovement;
    public AudioSource audioJump;
    public AudioSource audioAttack;

    // animator, navmesh agent, and rigidbody references
    private Animator anim;
    private NavMeshAgent agent;
    private Rigidbody rb;

    // timers
    private float lastAttackTime;
    private float lastJumpTime;
    // knockback
    private bool isKnockedBack;
    private float knockbackTimer;
    // attack
    private bool isAttacking;
    private float attackTimer;

    
    void Start()
    {
        // get components
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        // set starting health
        currentHealth = maxHealth;
        UpdateHealthSlider();

        // navmesh settings for 2.5d
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;
    }

    void Update()
    {
        if (player == null || agent == null || rb == null)
            return;

        if (!agent.isOnNavMesh)
            return;

        // knockback state
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            anim.SetBool("isWalking", false);

            // stop movement audio
            if (audioMovement != null && audioMovement.isPlaying)
                audioMovement.Stop();

            if (knockbackTimer <= 0f)
                isKnockedBack = false;

            return;
        }

        // attack state
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            anim.SetBool("isWalking", false);

            // stop movement audio
            if (audioMovement != null && audioMovement.isPlaying)
                audioMovement.Stop();

            if (attackTimer <= 0f)
                isAttacking = false;

            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // keep navmesh synced
        agent.nextPosition = transform.position;

        // face player
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            transform.localScale = new Vector3(1f, 1f, 1f);

        // jump if player is above
        if (Time.time >= lastJumpTime + jumpCooldown)
        {
            if (player.position.y > transform.position.y + playerAboveAmount)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // play jump audio
                if (audioJump != null)
                    audioJump.Play();

                lastJumpTime = Time.time;
            }
        }

        // chase player
        if (distance <= detectRange && distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            Vector3 moveDir = agent.desiredVelocity.normalized;

            rb.linearVelocity = new Vector3(
                moveDir.x * agent.speed,
                rb.linearVelocity.y,
                0f
            );
        }
        // attack player
        else if (distance <= attackRange)
        {
            agent.isStopped = true;
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        // idle
        else
        {
            agent.isStopped = true;
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }

        // movement audio
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            if (audioMovement != null && !audioMovement.isPlaying)
                audioMovement.Play();
        }
        else
        {
            if (audioMovement != null && audioMovement.isPlaying)
                audioMovement.Stop();
        }

        // walking animation
        anim.SetBool("isWalking", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
    }

    void Attack()
    {
        // trigger attack animation
        anim.SetTrigger("Attack");

        // play attack audio
        if (audioAttack != null)
            audioAttack.Play();

        isAttacking = true;
        attackTimer = attackDuration;

        PlayerController playerHealth = player.GetComponent<PlayerController>();
        if (playerHealth != null)
            playerHealth.TakeDamage(attackDamage);

        // apply knockback to player
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;

            playerRb.AddForce(
                new Vector3(dir.x * knockBackForce, knockbackUpForce, 0f),ForceMode.Impulse);
        }
    }

    public void ApplyKnockback(Vector3 attackerPosition)
    {
        // apply knockback to enemy
        Vector3 dir = (transform.position - attackerPosition).normalized;

        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        rb.AddForce(
            new Vector3(dir.x * knockBackForce, knockbackUpForce, 0f),
            ForceMode.Impulse
        );

        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
    }

    public void TakeDamage(float amount)
    {
        // reduce health
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthSlider();

        if (currentHealth <= 0)
            Die();
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
        // use game manager method
        GameManager.instance.PlayerWonRound();
    }

    public void DeathAnimationEnd()
    {
        if (GameManager.instance != null)
            GameManager.instance.EnemyWonRound();
    }
    

    void OnTriggerEnter(Collider other)
    {
        // die if falling into void
        if (other.CompareTag("Void"))
            Die();
    }
}