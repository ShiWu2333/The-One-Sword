using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public event EventHandler OnPlayerDie;
    
    [SerializeField] Animator animator;
    [SerializeField] PlayerReflectModeUI playerReflectModeUI;
    [SerializeField] BulletReflect bulletReflect;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject reflectEffect;
    [SerializeField] GameObject splashEffect;

    //Ѫ�����
    public SpriteRenderer[] hearts;
    public Sprite fullHeartSprite; // ���ĵ�ͼƬ
    public Sprite emptyHeartSprite; // ���ĵ�ͼƬ
    public float playerHealth; //�������ֵ

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
    [SerializeField] float reflectModeMax = 100;
    private float reflectModeChargeFill;
    private float damageBoost;


    void Start()
    {
        isReflectMode = true;
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
        if (reflectModeCharge >= 0) //���볬��ģʽ
        {
            reflectModeChargeFill = reflectModeCharge / reflectModeMax; // ���³��ܵ�UI, ����ұ����е��߼������ұ�����ʱ��reflectModeCharge�ͻ���0,��������������ӵ��������reflectModeCharge
            if (reflectModeCharge >=0 & reflectModeCharge  < 20)
            {
                damageBoost = 1;
            }
            else if (reflectModeCharge >= reflectModeMax * 1 / 5 &  reflectModeCharge < reflectModeMax * 2 / 5)
            {
                damageBoost = 1.2f;
            }
            else if (reflectModeCharge >= reflectModeMax * 2 / 5 & reflectModeCharge < reflectModeMax * 3 / 5)
            {
                damageBoost = 1.5f;
            }
            else if (reflectModeCharge >= reflectModeMax * 3 / 5 & reflectModeCharge < reflectModeMax * 4 / 5)
            {
                damageBoost = 2f;
            }
            else if (reflectModeCharge >= reflectModeMax * 4 / 5 & reflectModeCharge <= reflectModeMax)
            {
                damageBoost = 3f;
            }
            if (reflectModeCharge >= reflectModeMax)
            {
                canCharge = false;
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

                // �������ʱ�����ڵ���0.3�룬�����ṥ��
                if (heldTime <= 0.3f)
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

                if (heldTime > 0.3f)
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

    void ReflectBullet() // �����ӵ���ͬʱ�޸� bulletDamage ����
    {
        BaseBullet newBullet = Instantiate(bulletReflect, spawnPoint.transform.position, spawnPoint.transform.rotation)
                               .GetComponent<BaseBullet>();

        if (newBullet != null)
        {
            newBullet.bulletDamage = newBullet.bulletDamage * damageBoost; // �����µ��˺�ֵ
            newBullet.transform.localScale = newBullet.transform.localScale * damageBoost;
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
                if (bullet.bulletType == BaseBullet.BulletType.NomralBullet) //������е�����ͨ�ӵ�
                {
                    Debug.Log("normal bullet detected");
                    bullet.OnHit(); //�������ͨ�ӵ�������ֱ������
                    Instantiate(reflectEffect, bullet.transform.position, Quaternion.identity); //������Ч
                    Instantiate(splashEffect, bullet.transform.position, Quaternion.identity); //������Ч
                    hitSword = true;

                    if (isReflectMode) //����Ƿ���ģʽ�κι��������Է���
                    {
                        ReflectBullet();
                    }
                    if (canCharge) //������Գ�����ӳ���
                    {
                        reflectModeCharge += 1;
                        Debug.Log(reflectModeCharge);
                    };
                }
                else if (bullet.bulletType == BaseBullet.BulletType.HeavyBullet) //������е��������ӵ�
                {
                    Debug.Log("heavy bullet detected");
                    Instantiate(reflectEffect, bullet.transform.position, Quaternion.identity); //������Ч
                    Instantiate(splashEffect, bullet.transform.position, Quaternion.identity); //������Ч
                    hitSword = true;
                    bullet.OnHit();

                    if (isReflectMode)
                    {
                        ReflectBullet();
                    }
                    else if (isHeavyAttack)
                    {
                        if (canCharge) //������Գ�����ӳ���
                        {
                            reflectModeCharge += 1;
                            Debug.Log(reflectModeCharge);
                        };
                    }
                }
            }
        }

        if (!hitSword && healthCollider.enabled)
        {
            BaseBullet bullet = other.GetComponent<BaseBullet>();
            if (bullet != null)
            {
                playerHealth -= bullet.bulletDamage;
                //reflectModeCharge = 0;
                //canCharge = true;
                Debug.Log("Player hit! Remaining health: " + playerHealth);

                Destroy(other.gameObject);

                // ����������������������������ص��߼�
                if (playerHealth <= 0)
                {
                    OnPlayerDie?.Invoke(this, EventArgs.Empty);
                    //Debug.Log("Player is dead!");
                }
            }
        }
    }

    public float GetReflectModeChargeFill()
    { 
        return reflectModeChargeFill;
    }
}
