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
    [SerializeField] private AnimationCurve bleedDamageEachWave;
    [SerializeField] private AnimationCurve bleedTickEachWave;

    public int BleedDamage { get; private set; }
    public int BleedTick { get; private set; }

    public override void SetupStats(int currentWave)
    {
        base.SetupStats(currentWave);

        BleedDamage = (int)bleedDamageEachWave.Evaluate(currentWave);
        BleedTick = (int)bleedTickEachWave.Evaluate(currentWave);
    }

    protected override void AddKeys()
    {
        base.AddKeys();

        AddBleedDamageKey();
        AddBleedTickKey();
    }

    protected override void ClearCurves()
    {
        base.ClearCurves();

        bleedDamageEachWave = new AnimationCurve();
        bleedTickEachWave = new AnimationCurve();
    }

    protected override void AddHealthKey()
    {
        maxHPEachWave.AddKey(1, 30);            // Start with 30 HP
        maxHPEachWave.AddKey(25, 150);          // +5 each wave
        maxHPEachWave.AddKey(100, 900);         // +10 each wave
    }

    protected override void AddDamgeKey()
    {
        damageEachWave.AddKey(1, 5);            // Start with 5 damage
        damageEachWave.AddKey(100, 500);        // +5 each wave
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

    private void AddBleedDamageKey()
    {
        bleedDamageEachWave.AddKey(1, 1);       // Start with 1 bleed damage
        bleedDamageEachWave.AddKey(100, 100);   // +1 each wave
    }

    private void AddBleedTickKey()
    {
        bleedTickEachWave.AddKey(1, 3);
        bleedTickEachWave.AddKey(30, 4);
        bleedTickEachWave.AddKey(70, 5);
        MakeConstantValue(bleedTickEachWave);
    }
}
