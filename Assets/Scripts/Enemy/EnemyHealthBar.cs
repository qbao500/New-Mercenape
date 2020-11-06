using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Bao: Individual health bar for each enemy
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject slider;
    [SerializeField] private Slider sliderFunction;
    private float xScaleUI;

    private void Awake()
    {
        xScaleUI = healthBarUI.transform.localScale.x;
    }

    public void UpdateHealthBar(float currentHP, float maxHP)
    {       
        /*if (!slider.activeSelf)
        {
            StartCoroutine(HealthBarAnimation());
        }*/

        sliderFunction.value = currentHP / maxHP;       
    }

    public void ScaleRightUI(Rigidbody rb)
    {
        healthBarUI.transform.localScale = new Vector3(-(Mathf.Sign(rb.velocity.x)) * xScaleUI, healthBarUI.transform.localScale.y);
    }

    public void ScaleLeftUI(Rigidbody rb)
    {
        healthBarUI.transform.localScale = new Vector3((Mathf.Sign(rb.velocity.x)) * xScaleUI, healthBarUI.transform.localScale.y);
    }

    /*private IEnumerator HealthBarAnimation()
    {
        slider.SetActive(true);

        yield return new WaitForSeconds(1f);

        slider.SetActive(false);
    }*/
}