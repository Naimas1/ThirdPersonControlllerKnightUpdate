using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ������� ���������
    [SerializeField]
    private Animator playerAnim;

    // ��������� �� ��� � ����� �� �� �����
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject swordOnShoulder;

    // ���� ����������
    public bool isEquipping;
    public bool isEquipped;

    // ���� ����������, ������, �����
    public bool isBlocking;
    public bool isKicking;
    public bool isAttacking;

    // ��� � ������� �������� �����
    private float timeSinceAttack;

    // ������� ����� � ��������� (1, 2, 3)
    public int currentAttack = 0;

    private void Update()
    {
        // ��������� ������� �����
        timeSinceAttack += Time.deltaTime;

        // ������� �� ������
        Attack();
        Equip();
        Block();
        Kick();
    }

    // ����� ���������� / ������ ����
    private void Equip()
    {
        // ��� ��������� R � ���� ������� �� ����
        if (Input.GetKeyDown(KeyCode.R) && playerAnim.GetBool("Grounded"))
        {
            isEquipping = true;
            playerAnim.SetTrigger("Equip");
        }
    }

    // ����� ����������� ���������� ������� ��� ��������� / ��������� ����
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

    // ����� ����������� ���� ���������� ������� ����������
    public void Equipped()
    {
        isEquipping = false;
    }

    // ������� ����������
    private void Block()
    {
        // ���� ��������� ��� � ������� �� ����
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

    // ������� ����� ����� (��)
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

    // ������� ����� �����
    private void Attack()
    {
        // ���� ��� ���������, ������� �� ����, � ������� ��������� ���� � �������� �����
        if (Input.GetMouseButtonDown(0) && playerAnim.GetBool("Grounded") && timeSinceAttack > 0.8f)
        {
            if (!isEquipped)
                return;

            currentAttack++;
            isAttacking = true;

            // ��������� ������� ���� � ��������� (�� 3)
            if (currentAttack > 3)
                currentAttack = 1;

            // �������� ���������, ���� ����� �����������
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // ³������� ������� ����� � Animator
            playerAnim.SetTrigger("Attack" + currentAttack);

            // �������� �������
            timeSinceAttack = 0;
        }
    }

    // ����������� ���� ���������� ������� �����
    public void ResetAttack()
    {
        isAttacking = false;
    }
}
