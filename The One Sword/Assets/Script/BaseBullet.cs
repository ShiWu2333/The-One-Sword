using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
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

}
