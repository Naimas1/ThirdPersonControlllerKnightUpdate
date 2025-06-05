using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;

    public void ApplyDamage(float damage, Vector3 direction)
    {
        // Перевірка: якщо це гравець із блоком
        PlayerController player = GetComponent<PlayerController>();
        if (player != null && player.isBlocking)
        {
            Debug.Log("🛡️ Удар заблоковано щитом!");

            // ▶️ Звук блокування
            ShieldBlock shield = player.GetComponent<ShieldBlock>();
            if (shield != null)
            {
                shield.PlayBlockEffect();
            }

            // 💢 Відштовхування атакуючого NPC
            if (direction != Vector3.zero)
            {
                // напрямок атаки — навпаки від вектора
                Vector3 pushBack = -direction.normalized;
                Rigidbody rb = GetComponentInParent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                    rb.AddForce(pushBack * 200f, ForceMode.Impulse);
                }
            }

            return; // Урон не наноситься
        }

        // Якщо не блокує — нанести шкоду
        if (health != null)
        {
            health.TakeDamage(damage, direction);
            Debug.Log($" Успішна атака: -{damage} HP по {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($" HitBox на {gameObject.name} не має призначеного Health!");
        }
    }
}
