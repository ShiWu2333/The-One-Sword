using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVisual : MonoBehaviour
{
    [SerializeField] private Transform bulletVisual;
    [SerializeField] private Transform bulletShadow;
    [SerializeField] private BaseBullet bullet;
    [SerializeField] private float bulletRotation;

    private void Update()
    {
        UpdateBulletRotation();
    }

    private void UpdateBulletRotation()
    {
        Vector3 bulletMoveDir = bullet.GetBulletMoveDir();

        bulletVisual.transform.rotation = Quaternion.Euler(0, 0, bulletRotation + Mathf.Atan2(bulletMoveDir.y, bulletMoveDir.x) * Mathf.Rad2Deg - 90f);
    }

}
