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

    // State functions
    public void ChangeToInactive() => currentState = ForceFieldState.Inactive;

    public void ChangeToGenerating() => currentState = ForceFieldState.Generating;

    public void ChangeToActive() => currentState = ForceFieldState.Active;

    public void ChangeToDestroyed() => currentState = ForceFieldState.Destroyed;

    public bool IsInactive => currentState == ForceFieldState.Inactive;

    public bool IsGenerating => currentState == ForceFieldState.Generating;

    public bool IsActive => currentState == ForceFieldState.Active;

    public bool IsDestroyed => currentState == ForceFieldState.Destroyed;
}
