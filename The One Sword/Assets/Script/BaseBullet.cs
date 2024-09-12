using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public enum BulletType { NomralBullet, HeavyBullet}
    public BulletType bulletType;

    // �鷽����������Ը����������
    public virtual void OnHit()
    {
        // Ĭ����Ϊ���ӵ�������ʱ����
        Debug.Log("Bullet hit, destroyed.");
        Destroy(gameObject); // �����ӵ�
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
