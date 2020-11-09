using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Bao: Individual health bar for each enemy
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private Transform bar;
    [SerializeField] private float updateSpdSec = 0.2f;
    private float xScaleUI;

    private void Awake()
    {
        xScaleUI = healthBarUI.transform.localScale.x;
    }

    public void UpdateHealthBar(float currentHP, float maxHP)
    {
        var hpPct = currentHP / maxHP;
        hpPct = Mathf.Clamp(hpPct, 0, maxHP);
        if (hpPct < 1)
        {
            StartCoroutine(ChangeToPct(hpPct));
        }  
        else
        {   // Don't start coroutine animation when enemy is full HP
            bar.localScale = new Vector3(hpPct, bar.localScale.y);
        }
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = bar.localScale.x;
        float elapsed = 0f; 

        while (elapsed < updateSpdSec)
        {
            elapsed += Time.deltaTime;
            bar.localScale = new Vector3(Mathf.Lerp(preChangePct, pct, elapsed / updateSpdSec), bar.localScale.y);
            yield return null;
        }

        bar.localScale = new Vector3(pct, bar.localScale.y);
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