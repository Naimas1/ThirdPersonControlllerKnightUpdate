using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2.0f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    private Transform player;
    private Animator animator;
    private float lastAttackTime;
    private PlayerHealth playerHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown;

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null || playerHealth == null || playerHealth.IsDead)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            // Перевірка напрямку: атакуємо лише, якщо гравець перед NPC
            Vector3 toPlayer = (player.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toPlayer);

            if (dot > 0f) // гравець у передній півсфері
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null && playerController.isBlocking)
            {
                Debug.Log("🛡️ Гравець заблокував атаку!");

                ShieldBlock shield = player.GetComponent<ShieldBlock>();
                if (shield != null)
                {
                    shield.PlayBlockEffect();
                }

                Vector3 pushDirection = (transform.position - player.position).normalized;
                pushDirection.y = 0;
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                    rb.AddForce(pushDirection * 200f, ForceMode.Impulse);
                }

                return;
            }

            if (playerHealth != null && !playerHealth.IsDead)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}

