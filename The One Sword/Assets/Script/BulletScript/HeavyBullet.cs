using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBullet : BaseBullet
{
    public Vector2 initialVelocity = new Vector2(5, 10); // 初速度 (2D)
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        // 直接设置初速度
        rb2D.velocity = initialVelocity;
    }

    private void Update()
    {
        if ( transform.position.y < -10 )
        {
            Destroy(gameObject);
        }
    }

}
