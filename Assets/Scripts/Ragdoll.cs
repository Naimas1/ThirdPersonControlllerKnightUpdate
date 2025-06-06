using UnityEngine;

// Скрипт керування режимом Ragdoll (фізична симуляція тіла після смерті)
public class Ragdoll : MonoBehaviour
{
    // Масив усіх Rigidbody на дочірніх об'єктах
    private Rigidbody[] rigidbodies;

    // Масив усіх Collider на дочірніх об'єктах
    private Collider[] colliders;

    // Аніматор персонажа
    private Animator animator;

    void Start()
    {
        // Ініціалізація масивів і компонентів
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        animator = GetComponent<Animator>();

        // На початку вимикаємо ragdoll
        DeactivateRagdoll();
    }

    // Метод деактивує ragdoll (для звичайної анімації)
    public void DeactivateRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            if (rb != null)
                rb.isKinematic = true; // відключаємо фізику
        }

        foreach (var col in colliders)
        {
            if (col != null && col.gameObject != this.gameObject)
                col.enabled = false; // вимикаємо другорядні колайдери
        }

        if (animator != null)
            animator.enabled = true; // включаємо анімацію
    }

    // Метод активує ragdoll (наприклад, після смерті)
    public void ActivateRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            if (rb != null)
                rb.isKinematic = false; // вмикаємо фізику
        }

        foreach (var col in colliders)
        {
            if (col != null && col.gameObject != this.gameObject)
                col.enabled = true; // вмикаємо всі колайдери
        }

        if (animator != null)
            animator.enabled = false; // вимикаємо анімацію

        Debug.Log(" Ворог переведений у стан ragdoll.");
    }
}
