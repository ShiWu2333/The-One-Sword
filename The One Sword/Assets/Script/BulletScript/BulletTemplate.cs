using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fire Template", menuName = "Fire Template", order = 1)]
public class BulletTemplate : ScriptableObject
{
    public float[] fireIntervals;
    public BulletSpawner.BulletType[] bulletSequence;
}
