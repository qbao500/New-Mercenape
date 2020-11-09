using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingUpgrade : MonoBehaviour
{
    PlayerCurrency playerCurrency;
    TextMeshPro textMesh;

    private int upgradeAmount, sortingOrder;

    //public GameObject destination;
    public float speed, destroyTime;

    void Awake()
    {
        playerCurrency = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerCurrency>();
        //destination = GameObject.FindGameObjectWithTag("ResourceDestination");

        textMesh = textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        sortingOrder = 15;
        textMesh.sortingOrder = sortingOrder;
        textMesh.SetText("You found a rare weapon upgrade!");

    }

    private void OnEnable()
    {
        AddUpgrades();
        Invoke("Off", destroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void AddUpgrades()
    {
        upgradeAmount = 1;
        playerCurrency.AddUpgrades(upgradeAmount);
    }

    void Off()
    {
        gameObject.SetActive(false);
    }
}
