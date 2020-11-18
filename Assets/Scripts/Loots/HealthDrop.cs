using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    private PlayerHealth playerHealth;

    public int healingAmount;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (playerHealth.PlayerHP != playerHealth.PlayerMaxHP && collision.CompareTag("Player"))
        {
            playerHealth.GainHealth(healingAmount);
            gameObject.SetActive(false);
        }
    }

   
}
