using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public float damage = 25f;
    private bool canDealDamage = false;

    private Collider swordCollider;
    private bool isWeaponDrawn = false;

    void Start()
    {
        swordCollider = GetComponent<Collider>();
        if (swordCollider != null)
            swordCollider.enabled = false; // Початково вимкнено
    }

    void Update()
    {
        // Якщо натиснуто R — перемикаємо стан зброї
        if (Input.GetKeyDown(KeyCode.R))
        {
            isWeaponDrawn = !isWeaponDrawn;

            if (swordCollider != null)
                swordCollider.enabled = isWeaponDrawn;

            Debug.Log("Sword collider is now " + (isWeaponDrawn ? "enabled" : "disabled"));
        }
    }

    public void EnableDamage()
    {
        canDealDamage = true;
    }

    public void DisableDamage()
    {
        canDealDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage) return;

        HitBox hitBox = other.GetComponentInParent<HitBox>();
        if (hitBox != null)
        {
            Vector3 direction = other.transform.position - transform.position;
            direction.Normalize();

            hitBox.ApplyDamage(damage, direction);
            Debug.Log($"Атака по {other.name}: -{damage} HP");

            canDealDamage = false;
        }
    }
}
