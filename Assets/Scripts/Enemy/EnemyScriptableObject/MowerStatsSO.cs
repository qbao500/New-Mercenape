using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mower", menuName = "Enemy Stats/Mower Stats")]
public class MowerStatsSO : EnemyStatsSO
{
    [Header("Generator stats")]
    public float fieldMaxHP;
    public int fieldDamage;

    [Header("Generator states")]
    public ForceFieldState currentState;
    public enum ForceFieldState { Inactive, Generating, Active, Destroyed }

    protected override void AddHealthKey()
    {
        base.AddHealthKey();

        maxHPEachWave.AddKey(1, 80);        // Start with 80 HP
        maxHPEachWave.AddKey(15, 150);      // +5 each wave
        maxHPEachWave.AddKey(20, 200);      // +10 each wave
        maxHPEachWave.AddKey(60, 1000);     // +20 each wave
        maxHPEachWave.AddKey(100, 2000);    // +25 each wave
    }

    protected override void AddDamgeKey()
    {
        base.AddDamgeKey();
        
        damageEachWave.AddKey(1, 10);       // Start with 10 damage
        damageEachWave.AddKey(100, 1000);   // +10 each wave
    }

    protected override void AddSpeedKey()
    {
        speedEachWave.AddKey(1, 8);
        speedEachWave.AddKey(30, 9);
        speedEachWave.AddKey(50, 10);       
        base.AddSpeedKey();
    }

    #region State Functions
    public void ChangeToInactive() => currentState = ForceFieldState.Inactive;

    public void ChangeToGenerating() => currentState = ForceFieldState.Generating;

    public void ChangeToActive() => currentState = ForceFieldState.Active;

    public void ChangeToDestroyed() => currentState = ForceFieldState.Destroyed;

    public bool IsInactive => currentState == ForceFieldState.Inactive;

    public bool IsGenerating => currentState == ForceFieldState.Generating;

    public bool IsActive => currentState == ForceFieldState.Active;

    public bool IsDestroyed => currentState == ForceFieldState.Destroyed;
    #endregion
}
