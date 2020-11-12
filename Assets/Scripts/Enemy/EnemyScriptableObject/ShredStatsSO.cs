using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shred", menuName = "Shred Stats")]
public class ShredStatsSO : EnemyStatsSO
{
    [Header("Shred stagger")]
    public float staggerDuration;

    [Header("Shred bleed")]
    public int bleedChance;
    public int bleedDamage;
    public int bleedTick;
}
