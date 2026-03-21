using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform player;
    public float detectRange = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int damage = 1;

    public int maxHealth = 5;
    public int currentHealth;
    public Slider healthSlider;

    private Animator anim;
    private NavMeshAgent agent;
    private float lastAttackTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;
        UpdateHealthSlider();

        if (agent != null)
        {
            agent.updateRotation = false;
        }
    }

    void Update()
    {
        if (player == null || agent == null)
            return;

        if (!agent.isOnNavMesh)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Face player
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            transform.localScale = new Vector3(1f, 1f, 1f);

        // Chase player
        if (distance <= detectRange && distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        // Attack player
        else if (distance <= attackRange)
        {
            agent.isStopped = true;

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        // Idle if too far away
        else
        {
            agent.isStopped = true;
        }

        // Walking animation based on actual movement
        if (anim != null)
        {
            if (agent.velocity.magnitude > 0.1f && !agent.isStopped)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
    }

    void Attack()
    {
        if (anim != null)
            anim.SetTrigger("Attack");

        PlayerController playerHealth = player.GetComponent<PlayerController>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damage);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthSlider();

        if (currentHealth <= 0)
            Die();
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
        Destroy(gameObject);
    }
}