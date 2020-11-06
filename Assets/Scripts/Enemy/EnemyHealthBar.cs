using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Bao: Individual health bar for each enemy
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private Transform bar;
    private float xScaleUI;

    private void Awake()
    {
        xScaleUI = healthBarUI.transform.localScale.x;
    }

    public void UpdateHealthBar(float currentHP, float maxHP)
    {
        var hp = currentHP / maxHP;
        if (hp < 0) { hp = 0; }
        bar.localScale = new Vector3(hp, bar.localScale.y);          
    }

    public void ScaleRightUI(Rigidbody rb)
    {
        healthBarUI.transform.localScale = new Vector3(-(Mathf.Sign(rb.velocity.x)) * xScaleUI, healthBarUI.transform.localScale.y);
    }

    public void ScaleLeftUI(Rigidbody rb)
    {
        healthBarUI.transform.localScale = new Vector3((Mathf.Sign(rb.velocity.x)) * xScaleUI, healthBarUI.transform.localScale.y);
    }

}