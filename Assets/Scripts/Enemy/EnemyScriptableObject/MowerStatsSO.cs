using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mower", menuName = "Enemy Stats/Mower Stats")]
public class MowerStatsSO : EnemyStatsSO
{
    [Header("Generator stats")]
    [SerializeField] private AnimationCurve fieldHPEachWave;
    [SerializeField] private AnimationCurve fieldDamageEachWave;

    [Header("Shield colors")]
    [ColorUsage(true, true)] public Color shieldNormal;
    [ColorUsage(true, true)] public Color shieldDamage;

    public float FieldMaxHP { get; private set; }
    public int FieldDamage { get; private set; }

    public override void SetupStats(int currentWave)
    {
        base.SetupStats(currentWave);

        FieldMaxHP = (int)fieldHPEachWave.Evaluate(currentWave);
        FieldDamage = (int)fieldDamageEachWave.Evaluate(currentWave);
    }

    protected override void AddKeys()
    {
        base.AddKeys();

        AddFieldHealthKey();
        AddFieldDamageKey();
    }

    protected override void ClearCurves()
    {
        base.ClearCurves();

        fieldHPEachWave = new AnimationCurve();
        fieldDamageEachWave = new AnimationCurve();
    }

    protected override void AddHealthKey()
    {
        maxHPEachWave.AddKey(1, 80);            // Start with 80 HP
        maxHPEachWave.AddKey(15, 150);          // +5 each wave
        maxHPEachWave.AddKey(20, 200);          // +10 each wave
        maxHPEachWave.AddKey(60, 1000);         // +20 each wave
        maxHPEachWave.AddKey(100, 2000);        // +25 each wave
    }

    protected override void AddDamgeKey()
    {     
        damageEachWave.AddKey(1, 10);           // Start with 10 damage
        damageEachWave.AddKey(100, 1000);       // +10 each wave
    }

    protected override void AddSpeedKey()
    {
        speedEachWave.AddKey(1, 8);
        speedEachWave.AddKey(30, 9);
        speedEachWave.AddKey(50, 10);       
        base.AddSpeedKey();
    }

    private void AddFieldHealthKey()
    {
        fieldHPEachWave.AddKey(1, 40);          // Start with 40 (half scale of Mower HP)
        fieldHPEachWave.AddKey(15, 75);         // +2.5 each wave
        fieldHPEachWave.AddKey(20, 100);        // +5 each wave
        fieldHPEachWave.AddKey(60, 500);        // +10 each wave
        fieldHPEachWave.AddKey(100, 1000);      // +12.5 each wave
    }

    private void AddFieldDamageKey()
    {
        fieldDamageEachWave.AddKey(1, 20);      // Start with 20 damage (double scale of Mower damage)
        fieldDamageEachWave.AddKey(100, 2000);  // +20 each wave
    }

    public void ShieldGenerating(Material mat)
    {
        mat.SetFloat("_FresnelPower", 10f);
        mat.SetFloat("_ScrollSpeed", 0.1f);
    }

    public void ShieldOn(Material mat)
    {
        mat.SetFloat("_FresnelPower", 2.5f);
        mat.SetFloat("_ScrollSpeed", 0.3f);
    }  

    public IEnumerator ShieldReflect(Material mat)
    {
        mat.SetColor("_Emission", shieldDamage);

        yield return new WaitForSeconds(.2f);

        mat.SetColor("_Emission", shieldNormal);
    }

}
