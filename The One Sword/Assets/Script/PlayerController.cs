using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    [SerializeField] BulletSpawner bulletSpawner;
    [SerializeField] Animator animator;
    [SerializeField] PlayerReflectModeUI playerReflectModeUI;
    [SerializeField] BulletReflect bulletReflect;
    [SerializeField] GameObject spawnPoint;

    //Ѫ�����
    public SpriteRenderer[] hearts;
    public Sprite fullHeartSprite; // ���ĵ�ͼƬ
    public Sprite emptyHeartSprite; // ���ĵ�ͼƬ
    private int playerHealth; //�������ֵ

    //�����ж��������
    public GameObject playerHitbox; //��ҵ���ײ���
    public GameObject attackHitbox; // ��������ײ���
    public Collider2D perfectHit; //�����񵲵�
    private Collider2D hitboxCollider;
    private Collider2D healthCollider;
    private float buttonPressTime; // ��¼�������µ�ʱ��
    private bool isPressingButton; // ��¼�����Ƿ񱻰���
    private bool isHeavyAttack;
    private bool isReflectMode;
    
    
    //����mode�������
    private float reflectModeCharge;
    private bool canCharge;
    [SerializeField] private float reflectModeMax;
    [SerializeField] float reflectModeDuration;
    private float chargeTimer;


    void Start()
    {
        isReflectMode = false;
        playerHealth = 3;
        reflectModeCharge = 0;
        canCharge = true;

        hitboxCollider = attackHitbox.GetComponent<Collider2D>();
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true; // ��ʼ����collider 2d
        }

        healthCollider = playerHitbox.GetComponent<Collider2D>();
        healthCollider.enabled = true;

    }

    void Update()
    {
        UpdateHealthUI();//�������Ѫ��
        playerReflectModeUI.UpdateChargeFill();//���³���mode UI
        int currentCombo = animator.GetInteger("Combo");
        //Debug.Log("CurentCombo = " + currentCombo);
        

        //��ҳ���mode�߼�
        if (reflectModeCharge >= reflectModeMax) //���볬��ģʽ
        {
            canCharge = false; //�����Լ�����������
            isReflectMode = true; //���Է�������
            chargeTimer = reflectModeDuration; //���õ���ʱ
            Debug.Log("Refelct Mode!!");
        }
        
        if (isReflectMode)
        {
            if (chargeTimer > 0) 
            {
                chargeTimer -= Time.deltaTime; //����ģʽ����ʱ
                reflectModeCharge = chargeTimer; //���ݵ���ʱ��̬����chargefill
                Debug.Log("Reflect Mode Time : " + chargeTimer);
            }
            else //�˳�����ģʽ
            {
                canCharge = true;
                isReflectMode = false;
                chargeTimer = 0;
                reflectModeCharge = 0;
                Debug.Log("Reflect Mode End!!");
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
                if (heldTime <= 0.5f)
                {
                    isHeavyAttack = false;
                    PerformLightAttack();
                    if (currentCombo == 0)
                    {
                        currentCombo = 1;
                    }
                    else
                    {
                        currentCombo = 0;
                    }
                    animator.SetInteger("Combo", currentCombo);
                }

                if (heldTime > 0.5f)
                {
                    isHeavyAttack = true;
                    PerformHeavyAttack();
                }

                isPressingButton = false;
            }
        }
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }

    void PerformHeavyAttack()
    {
        if (hitboxCollider !=null)
        {
            animator.SetTrigger("HeavyAttack");
        }
    }

    // ִ���ṥ��
    void PerformLightAttack()
    {
        if (hitboxCollider != null)
        {
            animator.SetTrigger("LightAttack");
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
            BaseBullet bullet = other.GetComponent<BaseBullet>();
            if (bullet != null)
            {
                if (isHeavyAttack || isReflectMode)
                {
                    bullet.OnHit();
                    Instantiate(bulletReflect, spawnPoint.transform.position, spawnPoint.transform.rotation);
                }
                else
                {
                    bullet.OnHit(); // �����ӵ�
                    //Debug.Log("Bullet destroyed by light attack!");

                    if (canCharge)
                    {
                        Vector2 bulletPosition = other.transform.position;

                        reflectModeCharge += 1;
                        Debug.Log(reflectModeCharge);

                    }

                }
                hitSword = true;
            }

        }

        if (!hitSword && healthCollider.enabled)
        {
            BaseBullet bullet = other.GetComponent<BaseBullet>();
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

    public float GetReflectModeCharge()
    { 
        return reflectModeCharge;
    }

    public float GetReflectModeMax()
    {
        return reflectModeMax;
    }

}
