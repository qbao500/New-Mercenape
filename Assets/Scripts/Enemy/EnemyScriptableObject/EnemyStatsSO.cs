using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Bao: store enemy basic stats
public class EnemyStatsSO : ScriptableObject
{
    [Header("Base stats")]
    public new string name;
    public int damage;
    public float maxHP;
    public float runningSpeed;
    public int spaceToGetUp;
}
