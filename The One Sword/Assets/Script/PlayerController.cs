using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public event EventHandler OnPlayerDie;
    public event EventHandler OnPlayerHit; //��һ����ӵ�ʱ
    public event EventHandler OnPlayerDamaged; // ��ұ��ӵ�����
    
    [SerializeField] Animator animator;
    [SerializeField] PlayerReflectModeUI playerReflectModeUI;
    [SerializeField] BulletReflect bulletReflect;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject hitEffects;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private float perfectHitDistance; // ���Ը����������
    [SerializeField] private Transform perfectHit; // ���� perfectHit ��λ�ã����ھ�׼���е��ж�

    //Ѫ�����
    public SpriteRenderer[] hearts;
    public Sprite fullHeartSprite; // ���ĵ�ͼƬ
    public Sprite emptyHeartSprite; // ���ĵ�ͼƬ
    public float playerHealth; //�������ֵ
    private bool playerIsImmune; //����Ƿ��޵�
    private bool playerIsDead; //����Ƿ�����

    //�����ж��������
    public GameObject playerHitbox; //��ҵ���ײ���
    public GameObject attackHitbox; // ��������ײ���
    private Collider2D hitboxCollider;
    private Collider2D healthCollider;
    private float buttonPressTime; // ��¼�������µ�ʱ��
    private bool isPressingButton; // ��¼�����Ƿ񱻰���
    private bool isHeavyAttack;
    private bool isReflectMode;
    public int attackCombo; //��ҵ�ǰ������
    public bool isPerfectHit;



    //����mode�������
    private float reflectModeCharge;
    private bool canCharge;
    [SerializeField] float reflectModeMax = 100;
    private float reflectModeChargeFill;
    private float damageBoost;


    void Start()
    {
        attackCombo = 0;
        playerIsImmune = false;
        playerIsDead = false;
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

        //Debug.Log("Imunity state is " + playerIsImmune);
        //Debug.Log("CurentCombo = " + currentCombo);

        //��ҳ���mode�߼�
        if (reflectModeCharge >= 0) //���볬��ģʽ
        {
            reflectModeChargeFill = reflectModeCharge / reflectModeMax; // ���³��ܵ�UI, ����ұ����е��߼������ұ�����ʱ��reflectModeCharge�ͻ���0,��������������ӵ��������reflectModeCharge
            if (reflectModeCharge >=0 & reflectModeCharge  < 20)
            {
                damageBoost = 1f;
            }
            else if (reflectModeCharge >= reflectModeMax * 1 / 5 &  reflectModeCharge < reflectModeMax * 2 / 5)
            {
                damageBoost = 1.1f;
            }
            else if (reflectModeCharge >= reflectModeMax * 2 / 5 & reflectModeCharge < reflectModeMax * 3 / 5)
            {
                damageBoost = 1.3f;
            }
            else if (reflectModeCharge >= reflectModeMax * 3 / 5 & reflectModeCharge < reflectModeMax * 4 / 5)
            {
                damageBoost = 1.5f;
            }
            else if (reflectModeCharge >= reflectModeMax * 4 / 5 & reflectModeCharge <= reflectModeMax)
            {
                damageBoost = 2f;
            }
            if (reflectModeCharge >= reflectModeMax)
            {
                canCharge = false;
            }
            
        }
        

        // ��ⰴ�����µ�ʱ��
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
        {
            buttonPressTime = Time.time; // ��¼����ʱ��
            isPressingButton = true;
        }

        // ��ⰴ���ɿ���ʱ��
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X))
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
            newBullet.transform.localScale = newBullet.transform.localScale * (damageBoost+0.75f);
        }
    }

    // �����ײ�����ݻ��ӵ�

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("bullet is detected");
        bool hitSword = false;
        

        if (hitboxCollider.enabled)
        {
            // ��ȡ������hitbox�е��ӵ����ҵ������һ��
            Collider2D[] colliders = Physics2D.OverlapBoxAll(hitboxCollider.bounds.center, hitboxCollider.bounds.size, 0);
            BaseBullet nearestBullet = null;
            float minDistance = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                BaseBullet bullet = collider.GetComponent<BaseBullet>();
                if (bullet != null)
                {
                    float distance = Vector2.Distance(transform.position, bullet.transform.position);
                    if (distance < minDistance)
                    {
                        nearestBullet = bullet;
                        minDistance = distance;
                    }
                }
            }

            if (nearestBullet != null && nearestBullet == other.GetComponent<BaseBullet>())
            {
                BaseBullet bullet = nearestBullet;

                float distanceToPerfectHit = Vector2.Distance(perfectHit.position, bullet.transform.position); //����ӵ����������е�ľ���
                if (distanceToPerfectHit < perfectHitDistance) //�趨�Ƿ�����������
                {
                    isPerfectHit = true;
                }
                else
                {
                    isPerfectHit = false;
                }

                OnPlayerHit?.Invoke(this, EventArgs.Empty); //�������ӵ�
                attackCombo += 1;
                Debug.Log("Current Attack Combo = " +  attackCombo);



                if (bullet.bulletType == BaseBullet.BulletType.NomralBullet) //������е�����ͨ�ӵ�
                {
                    //Debug.Log("normal bullet detected");
                    bullet.OnHit(); //�������ͨ�ӵ�������ֱ������
                    Instantiate(hitEffects, bullet.transform.position, Quaternion.identity); //������Ч
                    hitSword = true;
                    //Debug.Log("Bullet Hit Distance is " + distanceToPerfectHit);

                    if (canCharge) //������Գ�����ӳ���
                    {
                        if (isPerfectHit) // ����Ǿ�׼����
                        {
                            reflectModeCharge += 3;
                            //Debug.Log("Perfect Hit!");
                            // ������ִ�о�׼���е��߼�������ӷֻ�����Ч��
                        }
                        else //��ͨ����
                        {
                            reflectModeCharge += 1;
                            //Debug.Log("Normal hIT...");
                        }
                    }
                    
                    if (isReflectMode) //����Ƿ���ģʽ�κι��������Է���
                    {
                        ReflectBullet();
                    }
                }
                else if (bullet.bulletType == BaseBullet.BulletType.HeavyBullet) //������е��������ӵ�
                {
                    Debug.Log("heavy bullet detected");
                    Instantiate(hitEffects, bullet.transform.position, Quaternion.identity); //������Ч
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
                            if (isPerfectHit) // ����Ǿ�׼����
                            {
                                reflectModeCharge += 3;
                                //Debug.Log("Perfect Hit!");
                                // ������ִ�о�׼���е��߼�������ӷֻ�����Ч��
                            }
                            else //��ͨ����
                            {
                                reflectModeCharge += 1;
                                //Debug.Log("Normal hIT...");
                            }
                        }
                    }
                }
            }
        }

        if (hitSword == false && healthCollider.enabled) 
        {
            BaseBullet bullet = other.GetComponent<BaseBullet>();
            if (bullet != null)
            {
                if (playerIsImmune == false) //��ұ�����
                {
                    OnPlayerDamaged?.Invoke(this, EventArgs.Empty);
                    playerHealth -= bullet.bulletDamage;
                    playerIsImmune = true;
                    attackCombo = 0;

                    StartCoroutine(RemoveImmunity());
                    reflectModeCharge = 0;
                    canCharge = true;
                    Debug.Log("Player hit! Remaining health: " + playerHealth);
                    animator.SetTrigger("Hurt");
                }

                Destroy(other.gameObject);

                // ����������������������������ص��߼�
                if (playerIsDead == false)
                {
                    if (playerHealth <= 0)
                    {
                        OnPlayerDie?.Invoke(this, EventArgs.Empty);
                        //Debug.Log("Player is dead!");
                        playerIsDead = true;
                    }
                }
            }
        }
    }

    private IEnumerator RemoveImmunity()
    {
        float immunityDuration = 3f;    // ����ʱ��
        float flashInterval = 0.1f;     // ��˸���
        float elapsedTime = 0f;

        // ��������״̬
        playerIsImmune = true;

        // ����״̬�ڼ���˸
        while (elapsedTime < immunityDuration)
        {
            // �л���ɫ��͸��������
            spriteRenderer.color = new Color(1, 1, 1, 0.6f);  // ��͸��
            yield return new WaitForSeconds(flashInterval / 2);
            spriteRenderer.color = new Color(1, 1, 1, 1f);    // �ָ�ԭɫ
            yield return new WaitForSeconds(flashInterval / 2);

            elapsedTime += flashInterval;
        }

        // ������˸���ָ�����״̬
        spriteRenderer.color = new Color(1, 1, 1, 1f);

        // ����ʱ��������ر�����״̬
        playerIsImmune = false;
    }

    public float GetReflectModeChargeFill()
    { 
        return reflectModeChargeFill;
    }
}
