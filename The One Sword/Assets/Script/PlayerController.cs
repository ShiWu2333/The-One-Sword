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

    //血量相关
    public SpriteRenderer[] hearts;
    public Sprite fullHeartSprite; // 满心的图片
    public Sprite emptyHeartSprite; // 空心的图片
    public float playerHealth; //玩家生命值

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
        if (reflectModeCharge >= 0) //进入超能模式
        {
            reflectModeChargeFill = reflectModeCharge / reflectModeMax; // 更新充能的UI, 在玩家被击中的逻辑里，当玩家被击中时，reflectModeCharge就会变成0,当玩家完美击中子弹则会增加reflectModeCharge
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

                // 如果按下时间少于等于0.3秒，发动轻攻击
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

    // 执行轻攻击
    void PerformLightAttack()
    {
        if (hitboxCollider != null)
        {
            animator.SetTrigger("LightAttack");
        }
    }

    void ReflectBullet() // 生成子弹并同时修改 bulletDamage 属性
    {
        BaseBullet newBullet = Instantiate(bulletReflect, spawnPoint.transform.position, spawnPoint.transform.rotation)
                               .GetComponent<BaseBullet>();

        if (newBullet != null)
        {
            newBullet.bulletDamage = newBullet.bulletDamage * damageBoost; // 设置新的伤害值
            newBullet.transform.localScale = newBullet.transform.localScale * damageBoost;
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
                if (bullet.bulletType == BaseBullet.BulletType.NomralBullet) //如果击中的是普通子弹
                {
                    Debug.Log("normal bullet detected");
                    bullet.OnHit(); //如果是普通子弹被攻击直接销毁
                    Instantiate(reflectEffect, bullet.transform.position, Quaternion.identity); //攻击特效
                    Instantiate(splashEffect, bullet.transform.position, Quaternion.identity); //击中特效
                    hitSword = true;

                    if (isReflectMode) //如果是反弹模式任何攻击都可以反弹
                    {
                        ReflectBullet();
                    }
                    if (canCharge) //如果可以充能添加充能
                    {
                        reflectModeCharge += 1;
                        Debug.Log(reflectModeCharge);
                    };
                }
                else if (bullet.bulletType == BaseBullet.BulletType.HeavyBullet) //如果击中的是重型子弹
                {
                    Debug.Log("heavy bullet detected");
                    Instantiate(reflectEffect, bullet.transform.position, Quaternion.identity); //攻击特效
                    Instantiate(splashEffect, bullet.transform.position, Quaternion.identity); //击中特效
                    hitSword = true;
                    bullet.OnHit();

                    if (isReflectMode)
                    {
                        ReflectBullet();
                    }
                    else if (isHeavyAttack)
                    {
                        if (canCharge) //如果可以充能添加充能
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

                // 可以在这里添加玩家死亡或其他相关的逻辑
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
