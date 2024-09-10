using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    // 虚方法，子类可以覆盖这个方法
    public virtual void OnHit()
    {
        // 默认行为：子弹被击中时销毁
        Debug.Log("Bullet hit, destroyed.");
        Destroy(gameObject); // 销毁子弹
    }

    public virtual void ReflectHit()
    {
        Debug.Log("Bullet is reflected by heavy attack");
    }

}
