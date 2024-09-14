using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReflect : BaseBullet
{
    [SerializeField] float bulletSpeed;

    void Update()
    {
        transform.position += Vector3.right * bulletSpeed * Time.deltaTime;
    }
}
