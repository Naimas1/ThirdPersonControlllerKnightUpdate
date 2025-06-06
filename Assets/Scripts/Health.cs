using UnityEngine;
using UnityEngine.AI;


public class Health : MonoBehaviour
{
    public float maxHealth = 100f; // Максимальна кількість здоров'я
    [HideInInspector] public float currentHealth; // Поточне здоров'я (приховане в інспекторі)

    private Collider[] colliders;      // Всі дочірні колайдери (для ragdoll)
    private Rigidbody[] rigidbodies;   // Всі дочірні rigidbody (для ragdoll)
    private Animator animator;         // Аніматор NPC
    private bool isDead = false;       // Чи мертвий персонаж

    void Start()
    {
        // Ініціалізація здоров'я при старті
        currentHealth = maxHealth;

        // Збирання усіх компонентів ragdoll
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        // Вимикаємо фізику (rigidbody) та колайдери, крім головного тіла
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (var col in colliders)
        {
            if (col.gameObject != gameObject) // не головний об'єкт
                col.enabled = false;
        }
    }

    // Метод отримання шкоди
    public void TakeDamage(float amount, Vector3 direction)
    {
        if (isDead)
        {
            Debug.Log($" Атака неуспішна: 0 HP (вже мертвий)");
            return;
        }

        // Зменшуємо здоров’я, але не дозволяємо йому бути менше 0
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($" Успішна атака: -{amount} HP → Залишилось: {currentHealth} HP");

        if (currentHealth <= 0.0f)
        {
            Die(); // якщо HP = 0, викликаємо смерть
        }
    }

    // Обробка смерті
    private void Die()
    {
        isDead = true;

        // Активація тригера анімації смерті
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Вимикаємо компоненти, пов’язані з навігацією та пересуванням
        DisableMovementComponents();

        // Вимикаємо HitBox-и
        DisableHitBoxes();

        Debug.Log(" NPC помер — анімація смерті запущена, управління вимкнено.");
    }

    // Вимкнення колайдерів і компонентів навігації
    private void DisableMovementComponents()
    {
        var capsule = GetComponent<CapsuleCollider>();
        if (capsule != null) capsule.enabled = false;

        var controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        var nav = GetComponent<NavMeshAgent>();
        if (nav != null) nav.enabled = false;
    }

    // Вимкнення HitBox-колайдерів
    private void DisableHitBoxes()
    {
        HitBox[] hitboxes = GetComponentsInChildren<HitBox>();
        foreach (var hitbox in hitboxes)
        {
            Collider hitboxCollider = hitbox.GetComponent<Collider>();
            if (hitboxCollider != null)
                hitboxCollider.enabled = false;
        }
    }

    // Активація фізичного ragdoll-стану після смерті
    private void EnableRagdollAfterDeath()
    {
        // Вимикаємо Animator для уникнення конфліктів з ragdoll
        if (animator != null)
            animator.enabled = false;

        // Вмикаємо фізику та колайдери на всіх частинах тіла
        foreach (var rb in rigidbodies)
            rb.isKinematic = false;

        foreach (var col in colliders)
            col.enabled = true;

        Debug.Log(" Ragdoll активовано після анімації смерті.");
    }
}
