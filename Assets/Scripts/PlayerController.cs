using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Аніматор персонажа
    [SerializeField]
    private Animator playerAnim;

    // Посилання на меч у руках та на плечі
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject swordOnShoulder;

    // Стан екіпірування
    public bool isEquipping;
    public bool isEquipped;

    // Стан блокування, кікання, атаки
    public bool isBlocking;
    public bool isKicking;
    public bool isAttacking;

    // Час з моменту останньої атаки
    private float timeSinceAttack;

    // Поточна атака в комбінації (1, 2, 3)
    public int currentAttack = 0;

    private void Update()
    {
        // Оновлення таймера атаки
        timeSinceAttack += Time.deltaTime;

        // Обробка дій гравця
        Attack();
        Equip();
        Block();
        Kick();
    }

    // Метод екіпірування / зняття зброї
    private void Equip()
    {
        // При натисканні R і якщо гравець на землі
        if (Input.GetKeyDown(KeyCode.R) && playerAnim.GetBool("Grounded"))
        {
            isEquipping = true;
            playerAnim.SetTrigger("Equip");
        }
    }

    // Метод викликається анімаційним івентом для увімкнення / вимкнення зброї
    public void ActiveWeapon()
    {
        if (!isEquipped)
        {
            sword.SetActive(true);
            swordOnShoulder.SetActive(false);
            isEquipped = true;
        }
        else
        {
            sword.SetActive(false);
            swordOnShoulder.SetActive(true);
            isEquipped = false;
        }
    }

    // Метод викликається після завершення анімації екіпірування
    public void Equipped()
    {
        isEquipping = false;
    }

    // Обробка блокування
    private void Block()
    {
        // Якщо натиснуто ПКМ і гравець на землі
        if (Input.GetKey(KeyCode.Mouse1) && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Block", true);
            isBlocking = true;
        }
        else
        {
            playerAnim.SetBool("Block", false);
            isBlocking = false;
        }
    }

    // Обробка удару ногою (кік)
    public void Kick()
    {
        if (Input.GetKey(KeyCode.LeftControl) && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Kick", true);
            isKicking = true;
        }
        else
        {
            playerAnim.SetBool("Kick", false);
            isKicking = false;
        }
    }

    // Обробка атаки мечем
    private void Attack()
    {
        // Якщо ЛКМ натиснуто, гравець на землі, і пройшло достатньо часу з останньої атаки
        if (Input.GetMouseButtonDown(0) && playerAnim.GetBool("Grounded") && timeSinceAttack > 0.8f)
        {
            if (!isEquipped)
                return;

            currentAttack++;
            isAttacking = true;

            // Обмеження кількості атак у комбінації (до 3)
            if (currentAttack > 3)
                currentAttack = 1;

            // Скидання комбінації, якщо атака затрималась
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // Відправка тригера атаки в Animator
            playerAnim.SetTrigger("Attack" + currentAttack);

            // Скидання таймера
            timeSinceAttack = 0;
        }
    }

    // Викликається після завершення анімації атаки
    public void ResetAttack()
    {
        isAttacking = false;
    }
}
