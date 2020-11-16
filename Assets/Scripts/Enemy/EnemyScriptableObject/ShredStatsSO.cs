using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shred", menuName = "Enemy Stats/Shred Stats")]
public class ShredStatsSO : EnemyStatsSO
{
    [Header("Shred stagger")]
    public float staggerDuration;

    [Header("Shred bleed")]
    public int bleedChance;
    public int bleedDamage;
    public int bleedTick;

    protected override void AddHealthKey()
    {
        base.AddHealthKey();

        maxHPEachWave.AddKey(1, 30);        // Start with 30 HP
        maxHPEachWave.AddKey(25, 150);      // +5 each wave
        maxHPEachWave.AddKey(100, 900);     // +10 each wave
    }

    protected override void AddDamgeKey()
    {
        base.AddDamgeKey();

        damageEachWave.AddKey(1, 5);        // Start with 5 damage
        damageEachWave.AddKey(100, 500);    // +5 each wave
    }

    protected override void AddSpeedKey()
    {
        speedEachWave.AddKey(1, 20);
        speedEachWave.AddKey(20, 22);
        speedEachWave.AddKey(40, 24);
        speedEachWave.AddKey(60, 27);
        speedEachWave.AddKey(80, 30);
        base.AddSpeedKey();
    }
}
