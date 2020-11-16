using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Created by Bao: store enemy basic stats
public class EnemyStatsSO : ScriptableObject
{
    [Header("Base stats")]
    public new string name;
    public int damage;
    public float maxHP;
    public float runningSpeed;
    public int spaceToGetUp;

    // Pattern for waves
    [SerializeField] protected AnimationCurve damageEachWave;
    [SerializeField] protected AnimationCurve maxHPEachWave;
    [SerializeField] protected AnimationCurve speedEachWave;
    
    public virtual void SetupStats(int currentWave)
    {
        AddKeys();

        damage = (int)damageEachWave.Evaluate(currentWave);
        maxHP = (int)maxHPEachWave.Evaluate(currentWave);
        runningSpeed = (int)speedEachWave.Evaluate(currentWave);
    }

    protected virtual void AddKeys()
    {
        AddHealthKey();
        AddDamgeKey();
        AddSpeedKey();
    }

    protected virtual void AddHealthKey() { }

    protected virtual void AddDamgeKey() { }

    protected virtual void AddSpeedKey()
    {
        for (int i = 0; i < speedEachWave.length; i++)
        {
            AnimationUtility.SetKeyRightTangentMode(speedEachWave, i, AnimationUtility.TangentMode.Constant);
        }
    }
}
