using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public event EventHandler OnPlayerDie;
    public event EventHandler OnPlayerHit; //玩家击中子弹时
    public event EventHandler OnPlayerDamaged; // 玩家被子弹击中
    
    [SerializeField] Animator animator;
    [SerializeField] PlayerReflectModeUI playerReflectModeUI;
    [SerializeField] BulletReflect bulletReflect;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject hitEffects;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private float perfectHitDistance; // 可以根据需求调整
    [SerializeField] private Transform perfectHit; // 定义 perfectHit 的位置，用于精准击中的判定

    //血量相关
    public SpriteRenderer[] hearts;
    public Sprite fullHeartSprite; // 满心的图片
    public Sprite emptyHeartSprite; // 空心的图片
    public float playerHealth; //玩家生命值
    private bool playerIsImmune; //玩家是否无敌
    private bool playerIsDead; //玩家是否死亡

    //攻击判定相关数据
    public GameObject playerHitbox; //玩家的碰撞体积
    public GameObject attackHitbox; // 攻击的碰撞体积
    private Collider2D hitboxCollider;
    private Collider2D healthCollider;
    private float buttonPressTime; // 记录按键按下的时间
    private bool isPressingButton; // 记录按键是否被按下
    private bool isHeavyAttack;
    private bool isReflectMode;
    public int attackCombo; //玩家当前连击数
    public bool isPerfectHit;



    //超能mode相关数据
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

        //Debug.Log("Imunity state is " + playerIsImmune);
        //Debug.Log("CurentCombo = " + currentCombo);

        //玩家超能mode逻辑
        if (reflectModeCharge >= 0) //进入超能模式
        {
            reflectModeChargeFill = reflectModeCharge / reflectModeMax; // 更新充能的UI, 在玩家被击中的逻辑里，当玩家被击中时，reflectModeCharge就会变成0,当玩家完美击中子弹则会增加reflectModeCharge
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
        

        // 检测按键按下的时间
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
        {
            buttonPressTime = Time.time; // 记录按下时间
            isPressingButton = true;
        }

        // 检测按键松开的时间
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X))
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
            newBullet.transform.localScale = newBullet.transform.localScale * (damageBoost+0.75f);
        }
    }

    // 检测碰撞，并摧毁子弹

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("bullet is detected");
        bool hitSword = false;
        

        if (hitboxCollider.enabled)
        {
            // 获取所有在hitbox中的子弹并找到最近的一个
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

                float distanceToPerfectHit = Vector2.Distance(perfectHit.position, bullet.transform.position); //检测子弹到完美击中点的距离
                if (distanceToPerfectHit < perfectHitDistance) //设定是否是完美击中
                {
                    isPerfectHit = true;
                }
                else
                {
                    isPerfectHit = false;
                }

                OnPlayerHit?.Invoke(this, EventArgs.Empty); //击中了子弹
                attackCombo += 1;
                Debug.Log("Current Attack Combo = " +  attackCombo);



                if (bullet.bulletType == BaseBullet.BulletType.NomralBullet) //如果击中的是普通子弹
                {
                    //Debug.Log("normal bullet detected");
                    bullet.OnHit(); //如果是普通子弹被攻击直接销毁
                    Instantiate(hitEffects, bullet.transform.position, Quaternion.identity); //攻击特效
                    hitSword = true;
                    //Debug.Log("Bullet Hit Distance is " + distanceToPerfectHit);

                    if (canCharge) //如果可以充能添加充能
                    {
                        if (isPerfectHit) // 如果是精准击中
                        {
                            reflectModeCharge += 3;
                            //Debug.Log("Perfect Hit!");
                            // 在这里执行精准击中的逻辑，比如加分或特殊效果
                        }
                        else //普通命中
                        {
                            reflectModeCharge += 1;
                            //Debug.Log("Normal hIT...");
                        }
                    }
                    
                    if (isReflectMode) //如果是反弹模式任何攻击都可以反弹
                    {
                        ReflectBullet();
                    }
                }
                else if (bullet.bulletType == BaseBullet.BulletType.HeavyBullet) //如果击中的是重型子弹
                {
                    Debug.Log("heavy bullet detected");
                    Instantiate(hitEffects, bullet.transform.position, Quaternion.identity); //攻击特效
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
                            if (isPerfectHit) // 如果是精准击中
                            {
                                reflectModeCharge += 3;
                                //Debug.Log("Perfect Hit!");
                                // 在这里执行精准击中的逻辑，比如加分或特殊效果
                            }
                            else //普通命中
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
                if (playerIsImmune == false) //玩家被击中
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

                // 可以在这里添加玩家死亡或其他相关的逻辑
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
        float immunityDuration = 3f;    // 免疫时长
        float flashInterval = 0.1f;     // 闪烁间隔
        float elapsedTime = 0f;

        // 进入免疫状态
        playerIsImmune = true;

        // 免疫状态期间闪烁
        while (elapsedTime < immunityDuration)
        {
            // 切换颜色：透明与正常
            spriteRenderer.color = new Color(1, 1, 1, 0.6f);  // 变透明
            yield return new WaitForSeconds(flashInterval / 2);
            spriteRenderer.color = new Color(1, 1, 1, 1f);    // 恢复原色
            yield return new WaitForSeconds(flashInterval / 2);

            elapsedTime += flashInterval;
        }

        // 结束闪烁并恢复正常状态
        spriteRenderer.color = new Color(1, 1, 1, 1f);

        // 免疫时间结束，关闭免疫状态
        playerIsImmune = false;
    }

    public float GetReflectModeChargeFill()
    { 
        return reflectModeChargeFill;
    }
}
