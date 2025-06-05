using UnityEngine;

// Вимагаємо, щоб на об'єкті, де буде цей скрипт, обов'язково були Collider і Rigidbody
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class SwordDamageHandler : MonoBehaviour
{
    // Кількість шкоди, що наноситься при ударі
    public float damageAmount = 25f;

    private void Start()
    {
        // Отримуємо компонент Rigidbody і вимикаємо фізичну симуляцію (щоб не впливав на об'єкт)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // не взаємодіє з фізикою
        }

        // Отримуємо компонент Collider і встановлюємо як trigger (для виявлення зіткнень без фізичних реакцій)
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true; // дозволяє виявляти зіткнення без колізій
        }
    }

    // Подія викликається при вході іншого об'єкта в тригер
    private void OnTriggerEnter(Collider other)
    {
        // Пошук компонента HitBox на іншому об'єкті
        HitBox hitbox = other.GetComponent<HitBox>();
        if (hitbox != null)
        {
            // Завдання шкоди через HitBox
            hitbox.ApplyDamage(damageAmount, transform.forward);
            Debug.Log($"🗡️ Удар по {other.name}: нанесено {damageAmount} шкоди.");
        }
    }
}
