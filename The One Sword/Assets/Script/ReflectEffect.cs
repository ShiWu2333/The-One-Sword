using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float destoryTime;

    void Start()
    {
        Destroy(gameObject, destoryTime);
    }

}
