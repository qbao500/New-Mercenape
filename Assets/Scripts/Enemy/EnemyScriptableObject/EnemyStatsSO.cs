using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Bao: store enemy basic stats
public class EnemyStatsSO : ScriptableObject
{
    [Header("Base stats")]
    public new string name;   
    public int spaceToGetUp;
   
    // Pattern for waves
    [SerializeField] protected AnimationCurve damageEachWave;
    [SerializeField] protected AnimationCurve maxHPEachWave;
    [SerializeField] protected AnimationCurve speedEachWave;

    [Header("Text colors")]
    public Color damageColor;
    public Color bleedColor;

    // Properties
    public int Damage { get; private set; }
    public float MaxHP { get; private set; }
    public float RunningSpeed { get; private set; }

    // Call in spawner in the beginning and after complete a wave
    public virtual void SetupStats(int currentWave)
    {
        AddKeys();

        Damage = (int)damageEachWave.Evaluate(currentWave);
        MaxHP = (int)maxHPEachWave.Evaluate(currentWave);
        RunningSpeed = (int)speedEachWave.Evaluate(currentWave);
    }

    // Set up values
    protected virtual void AddKeys()
    {
        ClearCurves();        // For safety

        AddHealthKey();
        AddDamgeKey();
        AddSpeedKey();
    }

    // Reset value (in case if there's a change in pattern design)
    protected virtual void ClearCurves()
    {
        damageEachWave = new AnimationCurve();
        maxHPEachWave = new AnimationCurve();
        speedEachWave = new AnimationCurve();
    }

    protected virtual void AddHealthKey() { }

    protected virtual void AddDamgeKey() { }

    protected virtual void AddSpeedKey() => MakeConstantValue(speedEachWave);

    // Make value constant until it get changed (in specific wave), and keep constant with the new changed value
    protected void MakeConstantValue(AnimationCurve curve)
    {
        for (int i = 0; i < curve.length; i++)
        {
            Keyframe key = curve[i];
            key.inTangent = Mathf.Infinity;
            key.outTangent = Mathf.Infinity;
            curve.MoveKey(i, key);
        }
    }   
}
