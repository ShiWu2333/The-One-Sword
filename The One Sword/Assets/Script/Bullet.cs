using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int bulletSpeed = 1;
    
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * bulletSpeed * Time.deltaTime;

        if (transform.position.x < -12)
        {
            Destroy(gameObject);
        }
    }

    public void ReflectBullet()
    {
        bulletSpeed = bulletSpeed * -1;
        transform.Rotate(0, 0, 180);
        Debug.Log("Bullet is reflected by heavy attack");
    }

}
