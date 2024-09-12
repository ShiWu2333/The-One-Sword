using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public enum BulletType { NomralBullet, HeavyBullet}
    public BulletType bulletType;

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

    public virtual void InitializeBullet(Transform target, float bulletSpeed, float traMaxHeight)
    {
        Debug.Log("Bullet is Initialized");
    }

    public virtual void InitializeAniCurve(AnimationCurve yTraAniCurve, AnimationCurve axisCorrectionAniCurve, AnimationCurve speedAniCurve)
    {
        
    }

    public virtual Vector3 GetBulletMoveDir()
    { 
        return Vector3.zero;
    }

}
