using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;

    private bool isDead = false;
    public bool IsDead => isDead; // ← додано для доступу ззовні

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Гравець отримав урон: " + amount + ", current health: " + currentHealth);

        if (animator != null)
        {
            animator.SetTrigger("takeDamage");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player died");

        if (animator != null)
        {
            animator.SetTrigger("Player Death");
        }

        // Тут можна відключити управління, активувати Game Over UI і т.д.
    }
}
