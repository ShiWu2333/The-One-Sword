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

    //血量相关
    public SpriteRenderer[] hearts;
    public Sprite fullHeartSprite; // 满心的图片
    public Sprite emptyHeartSprite; // 空心的图片
    private int playerHealth; //玩家生命值

    //攻击判定相关数据
    public GameObject playerHitbox; //玩家的碰撞体积
    public GameObject attackHitbox; // 攻击的碰撞体积
    public Collider2D perfectHit; //完美格挡点
    private Collider2D hitboxCollider;
    private Collider2D healthCollider;
    private float buttonPressTime; // 记录按键按下的时间
    private bool isPressingButton; // 记录按键是否被按下
    private bool isHeavyAttack;
    private bool isReflectMode;
    
    
    //超能mode相关数据
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
            hitboxCollider.enabled = true; // 初始启用collider 2d
        }

        healthCollider = playerHitbox.GetComponent<Collider2D>();
        healthCollider.enabled = true;

    }

    void Update()
    {
        UpdateHealthUI();//更新玩家血量
        playerReflectModeUI.UpdateChargeFill();//更新超能mode UI
        int currentCombo = animator.GetInteger("Combo");
        //Debug.Log("CurentCombo = " + currentCombo);
        

        //玩家超能mode逻辑
        if (reflectModeCharge >= reflectModeMax) //进入超能模式
        {
            canCharge = false; //不可以继续叠加能量
            isReflectMode = true; //可以反弹攻击
            chargeTimer = reflectModeDuration; //设置倒计时
            Debug.Log("Refelct Mode!!");
        }
        
        if (isReflectMode)
        {
            if (chargeTimer > 0) 
            {
                chargeTimer -= Time.deltaTime; //超能模式倒计时
                reflectModeCharge = chargeTimer; //根据倒计时动态降低chargefill
                Debug.Log("Reflect Mode Time : " + chargeTimer);
            }
            else //退出超能模式
            {
                canCharge = true;
                isReflectMode = false;
                chargeTimer = 0;
                reflectModeCharge = 0;
                Debug.Log("Reflect Mode End!!");
            }
        }

        // 检测按键按下的时间
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonPressTime = Time.time; // 记录按下时间
            isPressingButton = true;
        }

        // 检测按键松开的时间
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isPressingButton)
            {
                float heldTime = Time.time - buttonPressTime; // 计算按键持续时间

                // 如果按下时间少于等于1秒，发动轻攻击
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

    // 执行轻攻击
    void PerformLightAttack()
    {
        if (hitboxCollider != null)
        {
            animator.SetTrigger("LightAttack");
        }
    }


    

    // 检测碰撞，并摧毁子弹

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("bullet is detected");
        bool hitSword = false;

        if (hitboxCollider.enabled)
        {
            // 检查碰撞对象是否为子弹
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
                    bullet.OnHit(); // 销毁子弹
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

                // 可以在这里添加玩家死亡或其他相关的逻辑
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
