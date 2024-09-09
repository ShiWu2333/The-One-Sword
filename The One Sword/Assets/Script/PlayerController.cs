using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] BulletSpawner bulletSpawner;
    [SerializeField] Bullet bullet;
    [SerializeField] Animator animator;

    public GameObject playerHitbox; //��ҵ���ײ���
    public GameObject attackHitbox; // ��������ײ���
    private Collider2D hitboxCollider;
    private Collider2D healthCollider;
    private float buttonPressTime; // ��¼�������µ�ʱ��
    private bool isPressingButton; // ��¼�����Ƿ񱻰���
    private bool isHeavyAttack;
    private bool isReflectMode;
    private int playerHealth; //�������ֵ
    
    private int reflectModeCharge;
    private float reflectModeTime = 0;


    void Start()
    {
        playerHealth = 3;
        reflectModeCharge = 0;

        hitboxCollider = attackHitbox.GetComponent<Collider2D>();
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false; // ��ʼʱ����Hitbox
        }

        healthCollider = playerHitbox.GetComponent<Collider2D>();
        healthCollider.enabled = true;

    }

    void Update()
    {
        if (reflectModeCharge >= 2)
        {
            isReflectMode = true;
            reflectModeTime = 3;
            reflectModeCharge = 0;
            if (reflectModeTime > 0)
            {
                reflectModeTime -= Time.deltaTime;
            }
            if (reflectModeTime <= 0)
            {
                reflectModeTime = 0;
                isReflectMode = false;
                Debug.Log("Reflect Mode End.");
            }
        }

        // ��ⰴ�����µ�ʱ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonPressTime = Time.time; // ��¼����ʱ��
            isPressingButton = true;
        }

        // ��ⰴ���ɿ���ʱ��
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isPressingButton)
            {
                float heldTime = Time.time - buttonPressTime; // ���㰴������ʱ��

                // �������ʱ�����ڵ���1�룬�����ṥ��
                if (heldTime <= 1f)
                {
                    isHeavyAttack = false;
                    PerformLightAttack();
                    animator.SetTrigger("LightAttack");
                }

                if (heldTime > 1f)
                {
                    isHeavyAttack = true;
                    PerformHeavyAttack();
                    animator.SetTrigger("HeavyAttack");
                }

                isPressingButton = false;
            }
        }
    }

    void PerformHeavyAttack()
    {
        if (hitboxCollider !=null)
        {
            hitboxCollider.enabled = true; // ����Hitbox
            Debug.Log("Heavy attack performed!");

            // �������������빥�����������߼�
            // ��ͣһ��ʱ������Hitbox
            Invoke("DisableHitbox", 0.1f); // 0.1������Hitbox����ֹ�������
        }
    }

    // ִ���ṥ��
    void PerformLightAttack()
    {
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true; // ����Hitbox
            Debug.Log("Light attack performed!");
            

            // �������������빥�����������߼�
            // ��ͣһ��ʱ������Hitbox
            Invoke("DisableHitbox", 0.1f); // 0.1������Hitbox����ֹ�������
        }
    }

    void PerformPowerLightAttack()
    {

    }


    // ����Hitbox
    void DisableHitbox()
    {
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }
    }

    // �����ײ�����ݻ��ӵ�
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("bullet is detected");
        bool hitSword = false;
        if (hitboxCollider.enabled)
        {
            // �����ײ�����Ƿ�Ϊ�ӵ�
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                if (isHeavyAttack || isReflectMode)
                {
                    bullet.ReflectBullet();
                }
                else
                {
                    Destroy(other.gameObject); // �����ӵ�
                    Debug.Log("Bullet destroyed by light attack!");
                    reflectModeCharge += 1;
                }
                hitSword = true;
            }

        }

        if (!hitSword && healthCollider.enabled)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                playerHealth -= 1;
                Debug.Log("Player hit! Remaining health: " + playerHealth);

                Destroy(other.gameObject);

                // ����������������������������ص��߼�
                if (playerHealth <= 0)
                {
                    Debug.Log("Player is dead!");

                }
            }
        }
    }
}
